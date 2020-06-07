using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// todo highlight
namespace Grab
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabber : MonoBehaviour
    {
        [Range(0, 10000)]
        [Tooltip("How much force the grip can resist before releasing the grabbed object")]
        public float breakForce = 5000;

        [Tooltip("Range in which the object is considered to be grabbed")]
        public float grabbedRadius = 0.1f;

        [Tooltip("Speed of pull applied distance grabbable objects")]
        public float grabSpeed = 10f;

        [Tooltip("Layer mask used for ray casts")]
        public LayerMask layerMask;

        [Tooltip("Called when a grabbable is released")]
        public GrabEvent onRelease;

        [Tooltip("Called when and only grabbable starts to move")]
        public GrabEvent onMove;

        [Tooltip("Called when a grabbable is grabbed")]
        public GrabEvent onGrab;

        private GrabbedGrabbable grabbed;
        private Grabbable previousClosest;

        private readonly HashSet<Grabbable> trackedGrabbables =
            new HashSet<Grabbable>();

        [Serializable]
        public class GrabEvent : UnityEvent<Grabbable>
        {
        }

        private class GrabbedGrabbable
        {
            public readonly Grabbable Grabbable;
            public FixedJoint Joint;
            public bool Moving;

            public GrabbedGrabbable(Grabbable grabbable, bool moving)
            {
                Grabbable = grabbable;
                Moving = moving;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, grabbedRadius);
        }

        /// <summary>
        /// Release currently grabbed grabbable.
        /// </summary>
        public void Release()
        {
            if (grabbed == null)
            {
                return;
            }

            if (grabbed.Joint != null)
            {
                Destroy(grabbed.Joint);
            }

            var grabbable = grabbed.Grabbable;
            grabbable.Body.isKinematic = false;
            grabbable.grabbedBy = null;

            onRelease.Invoke(grabbable);

            grabbed = null;
        }

        /// <summary>
        /// Grab closest available grabbable.
        /// </summary>
        public void Grab()
        {
            if (trackedGrabbables.Count == 0)
            {
                return;
            }

            Release();

            var grabbable = GetClosestGrabbable();
            if (grabbable == null || !IsInSight(grabbable))
            {
                return;
            }

            HandleMovingStart(grabbable);
        }

        private Grabbable GetClosestGrabbable()
        {
            if (trackedGrabbables.Count <= 1)
            {
                return trackedGrabbables.FirstOrDefault();
            }

            var position = transform.position;

            return trackedGrabbables.Aggregate((grabbableA, grabbableB) =>
            {
                var distanceA = Vector3.Distance(position, grabbableA.transform.position);
                var distanceB = Vector3.Distance(position, grabbableB.transform.position);

                return distanceA < distanceB ? grabbableA : grabbableB;
            });
        }

        private bool IsInSight(Component component)
        {
            var intersects = Physics.Linecast(
                transform.position,
                component.transform.position,
                out var hit,
                layerMask
            );

            return intersects && hit.collider.gameObject == component.gameObject;
        }

        private void UpdateGrabJointForce(Joint joint)
        {
            if (joint.currentForce.magnitude >= breakForce)
            {
                Release();
            }
        }

        private void MoveGrabbableTo(Grabbable grabbable, Vector3 from, Vector3 to)
        {
            var newPosition = Vector3.MoveTowards(from, to, Time.deltaTime * grabSpeed);
            grabbable.Body.MovePosition(newPosition);
        }

        private bool IsMove(Grabbable grabbable, Vector3 to)
        {
            var from = grabbable.Body.ClosestPointOnBounds(to);
            return Vector3.Distance(from, to) > grabbedRadius;
        }

        private void HandleMovingStart(Grabbable grabbable)
        {
            var otherGrabber = grabbable.grabbedBy;
            if (otherGrabber != null)
            {
                otherGrabber.Release();
            }

            grabbable.grabbedBy = this;
            grabbable.Body.isKinematic = true;

            grabbed = new GrabbedGrabbable(
                grabbable: grabbable,
                moving: true
            );

            if (IsMove(grabbable, transform.position))
            {
                onMove.Invoke(grabbable);
            }
        }

        private void HandleMovingEnd(Grabbable grabbable)
        {
            var joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = grabbable.Body;
            joint.breakForce = breakForce;

            grabbable.Body.isKinematic = false;

            grabbed.Moving = false;
            grabbed.Joint = joint;

            onGrab.Invoke(grabbable);
        }

        private void UpdateMoving(Grabbable grabbable)
        {
            var grabbablePosition = grabbable.transform.position;
            var position = transform.position;

            if (IsMove(grabbable, position))
            {
                MoveGrabbableTo(grabbable, grabbablePosition, position);
            }
            else
            {
                HandleMovingEnd(grabbable);
            }
        }

        private void UpdateGrabbableHighlight()
        {
            // TODO: NOT WORKING
            var closest = GetClosestGrabbable();
            if (closest != null && (closest.grabbedBy != null || !IsInSight(closest)))
            {
                if (previousClosest != null)
                {
                    previousClosest.onDeselected.Invoke();
                    previousClosest = null;
                }

                return;
            }

            if (previousClosest != null)
            {
                Debug.Log("Deselected");
                previousClosest.onDeselected.Invoke();
                previousClosest = null;
            }

            if (previousClosest != closest)
            {
                Debug.Log("Selected");
                closest.onSelected.Invoke();
                previousClosest = closest;
            }
        }

        private void FixedUpdate()
        {
            // Cleanup.
            if (trackedGrabbables.Count == 0)
            {
                Release();
            }

            if (grabbed != null)
            {
                if (grabbed.Joint != null)
                {
                    UpdateGrabJointForce(grabbed.Joint);
                }

                if (grabbed.Moving)
                {
                    UpdateMoving(grabbed.Grabbable);
                }
            }

            UpdateGrabbableHighlight();

            // Cleanup as next frame will re-add.
            trackedGrabbables.Clear();
        }

        private void OnTriggerStay(Collider other)
        {
            var grabbable = other.GetComponent<Grabbable>();
            if (grabbable == null)
            {
                return;
            }

            trackedGrabbables.Add(grabbable);
        }
    }
}

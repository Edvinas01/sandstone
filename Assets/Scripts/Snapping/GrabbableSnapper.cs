namespace Snapping
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public class GrabbableSnapper : MonoBehaviour
    {
        [Tooltip("Destination where to snap the object")]
        public Transform destination;

        [Tooltip("Called when grabbable snaps")]
        public Event onSnap;

        [Tooltip("Called when grabbable unsnaps")]
        public Event onUnsnap;

        private Grabbable snappedGrabbable;

        // TODO: just can't figure out states...
        public void OnEnter(Grabbable grabbable)
        {
        }

        public void OnExit(Grabbable grabbable)
        {
            if (grabbable.isGrabbed || destination.childCount > 0)
            {
                return;
            }
            
            SnapToDestination(destination);
        }

        private void SnapToDestination(Component target)
        {
            var targetTransform = target.transform;

            targetTransform.position = destination.position;
            targetTransform.rotation = destination.rotation;
            targetTransform.parent = destination;

            var targetRigidbody = target.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = true;
            }
        }

        [Serializable]
        public class Event : UnityEvent<Grabbable>
        {
        }
    }
}
namespace Snapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Rules;
    using UnityEngine;
    using UnityEngine.Events;

    public class GrabbableSnapZone : MonoBehaviour
    {
        [Tooltip("Rules that must match in order for grabbable to snap")]
        public List<Rule> rules;

        [Tooltip("Called when grabbable enters the snap")]
        public Event onEnter;

        [Tooltip("Called when grabbable exits the snap")]
        public Event onExit;

        private Grabbable trackedGrabbable;

        public void OnDrawGizmos()
        {
            if (trackedGrabbable == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                trackedGrabbable.transform.position,
                transform.position
            );
        }

        public void Update()
        {
            // Still grabbed.
            if (trackedGrabbable == null || trackedGrabbable.isGrabbed)
            {
                return;
            }

            onExit.Invoke(trackedGrabbable);
            trackedGrabbable = null;
        }

        public void OnTriggerStay(Collider other)
        {
            // Is tracked grabbable valid?.
            if (trackedGrabbable != null && trackedGrabbable.isGrabbed)
            {
                return;
            }

            var grabbable = GetGrabbable(other);

            // Is the new grabbable in-valid?
            if (grabbable == null || !grabbable.isGrabbed)
            {
                return;
            }

            // Are all of the rules valid?
            if (rules.Any(rule => !rule.Accepts(grabbable)))
            {
                return;
            }

            trackedGrabbable = grabbable;
            onEnter.Invoke(trackedGrabbable);
        }

        public void OnTriggerExit(Collider other)
        {
            var grabbable = GetGrabbable(other);

            // Did the tracked grabbable leave?
            if (grabbable == null || grabbable != trackedGrabbable)
            {
                return;
            }

            onExit.Invoke(trackedGrabbable);
            trackedGrabbable = null;
        }

        [Serializable]
        public class Event : UnityEvent<Grabbable>
        {
        }

        [CanBeNull]
        private static Grabbable GetGrabbable(Component component)
        {
            return component.gameObject.GetComponent<Grabbable>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class GrabbableSnapper : MonoBehaviour
    {
        [Tooltip("Game object containing snapping slot transforms")]
        public GameObject slotContainer;

        [Tooltip("Should grabbables be locked in once snapped")]
        public bool lockIn;

        [Tooltip("Scale of items once they're inside snap slots")]
        public float scale = 1f;

        [Tooltip("Called when a grabbable is released")]
        public SnapEvent onRelease;

        [Tooltip("Called when a grabbable is snapped")]
        public SnapEvent onSnap;

        private List<Slot> slots;

        [Serializable]
        public class SnapEvent : UnityEvent<GameObject>
        {
        }

        private class Slot
        {
            public Vector3 OriginalScale;
            public OVRGrabbable Grabbable;
            public Transform Transform;
        }

        /// <summary>
        /// Snap game object to an available slot if it has a grabbable component.
        /// </summary>
        public void Snap(GameObject obj)
        {
            var grabbable = obj.GetComponentInParent<OVRGrabbable>();
            if (grabbable != null)
            {
                Snap(grabbable);
            }
        }

        /// <summary>
        /// Snap grabbable to an available slot.
        /// </summary>
        public bool Snap(OVRGrabbable grabbable)
        {
            var emptySlot = GetEmptySlot();
            if (emptySlot == null || IsSnapped(grabbable))
            {
                return false;
            }

            Snap(emptySlot, grabbable);
            return true;

        }

        /// <returns>
        /// List of snapped grabbables.
        /// </returns>
        public IEnumerable<OVRGrabbable> GetSnapped()
        {
            return slots
                .Where(slot => slot.Grabbable != null)
                .Select(slot => slot.Grabbable);
        }

        private Slot GetEmptySlot()
        {
            return slots.FirstOrDefault(slot => slot.Grabbable == null);
        }

        private void Snap(Slot slot, OVRGrabbable grabbable)
        {
            if (grabbable.grabbedBy != null)
            {
                grabbable.grabbedBy.ForceRelease(grabbable);
            }

            grabbable.GetComponent<Rigidbody>().isKinematic = true;

            // Move to parent.
            var grabbableTransform = grabbable.transform;
            var grabbableScale = grabbableTransform.localScale;
            var slotTransform = slot.Transform;

            grabbableTransform.localScale *= scale;
            grabbableTransform.position = slotTransform.position;
            grabbableTransform.rotation = slotTransform.rotation;
            grabbableTransform.parent = slotTransform;

            // Store to slot.
            slot.OriginalScale = grabbableScale;
            slot.Grabbable = grabbable;

            onSnap.Invoke(grabbable.gameObject);
        }

        private bool IsSlotTransform(Transform tr)
        {
            var parent = tr.parent;
            return slots.Any(slot => slot.Transform == parent);
        }

        private void Release(Slot slot)
        {
            var grabbable = slot.Grabbable;
            var grabbableTransform = grabbable.transform;
            grabbableTransform.localScale = slot.OriginalScale;

            if (IsSlotTransform(grabbableTransform))
            {
                grabbableTransform.parent = null;
            }

            grabbableTransform.gameObject.SetActive(true);

            slot.OriginalScale = Vector3.zero;
            slot.Grabbable = null;

            onRelease.Invoke(grabbable.gameObject);
        }

        private void Awake()
        {
            slots = slotContainer
                .GetComponentsInChildren<Transform>()
                .Where(slotTransform => slotTransform.gameObject != slotContainer)
                .Select(slotTransform => new Slot
                {
                    OriginalScale = Vector3.zero,
                    Grabbable = null,
                    Transform = slotTransform
                })
                .ToList();
        }

        private void Update()
        {
            foreach (var slot in slots)
            {
                var grabbable = slot.Grabbable;
                if (grabbable != null && grabbable.isGrabbed)
                {
                    Release(slot);
                }
            }
        }

        private static bool IsSnapped(Component grabbable)
        {
            return grabbable.GetComponentInParent<GrabbableSnapper>() != null;
        }
    }
}

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

        [Tooltip("Scale of items once they're inside snap slots")]
        public float scale = 1f;

        [Tooltip("Called when a grabbable is released")]
        public UnityEvent onRelease;

        [Tooltip("Called when a grabbable is snapped")]
        public UnityEvent onSnap;

        private List<Slot> slots;

        private class Slot
        {
            public Vector3 OriginalScale;
            public OVRGrabbable Grabbable;
            public Transform Transform;
        }

        /// <summary>
        /// Change each grabbables game object active state.
        /// </summary>
        public void SetActiveGrabbables(bool active)
        {
            var grabbables = slots
                .Select(slot => slot.Grabbable)
                .Where(grabbable => grabbable != null);

            foreach (var grabbable in grabbables)
            {
                grabbable.gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// Store grabbable to an available slot.
        /// </summary>
        public void Snap(OVRGrabbable grabbable)
        {
            var emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                Snap(emptySlot, grabbable);
            }
        }

        private void Snap(Slot slot, OVRGrabbable grabbable)
        {
            // Cleanup OVR state.
            grabbable.grabbedBy.ForceRelease(grabbable);
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

            grabbable.gameObject.SetActive(false);

            onSnap.Invoke();
        }

        private Slot GetEmptySlot()
        {
            return slots.FirstOrDefault(slot => slot.Grabbable == null);
        }

        private void Release(Slot slot)
        {
            var grabbableTransform = slot.Grabbable.transform;
            grabbableTransform.localScale = slot.OriginalScale;
            grabbableTransform.parent = null;
            grabbableTransform.gameObject.SetActive(true);

            slot.OriginalScale = Vector3.zero;
            slot.Grabbable = null;

            onRelease.Invoke();
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
    }
}

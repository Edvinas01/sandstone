using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class GrabberInventory : MonoBehaviour
    {
        [Tooltip("Button which triggers inventory actions")]
        public OVRInput.Button trigger = OVRInput.Button.One;

        [Tooltip("Game object containing inventory slot transforms")]
        public GameObject slotContainer;

        [Tooltip("Assigned grabbed to this inventory")]
        public OVRGrabber grabber;

        [Tooltip("Scale of items once they're inside inventory slots")]
        public float scale = 0.5f;

        [Tooltip("Called when an item was stored")]
        public UnityEvent onStoreItem;

        private List<InventorySlot> slots;

        private class InventorySlot
        {
            public Vector3 OriginalScale;
            public OVRGrabbable Grabbable;
            public Transform Transform;
        }

        private static void Release(InventorySlot slot)
        {
            var grabbableTransform = slot.Grabbable.transform;
            grabbableTransform.localScale = slot.OriginalScale;
            grabbableTransform.parent = null;
            grabbableTransform.gameObject.SetActive(true);

            slot.OriginalScale = Vector3.zero;
            slot.Grabbable = null;
        }

        private void Store(InventorySlot slot, OVRGrabbable grabbable)
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

            onStoreItem.Invoke();
        }

        private void UpdateReleasedItems()
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

        private InventorySlot GetEmptySlot()
        {
            foreach (var slot in slots)
            {
                if (slot.Grabbable == null)
                {
                    return slot;
                }
            }

            return null;
        }

        private void SetShowItems(bool show)
        {
            foreach (var slot in slots)
            {
                var grabbable = slot.Grabbable;
                if (grabbable != null)
                {
                    grabbable.gameObject.SetActive(show);
                }
            }
        }

        private void UpdateInput()
        {
            var controller = grabber.Controller;
            if (OVRInput.GetDown(trigger, controller))
            {
                var grabbable = grabber.grabbedObject;
                if (grabbable != null && grabbable.allowInventory)
                {
                    var emptySlot = GetEmptySlot();
                    if (emptySlot != null)
                    {
                        Store(emptySlot, grabbable);
                    }

                    return;
                }

                SetShowItems(true);
            }
            else if (OVRInput.GetUp(trigger, controller))
            {
                SetShowItems(false);
            }
        }

        private void Awake()
        {
            slots = slotContainer
                .GetComponentsInChildren<Transform>()
                .Where(slotTransform => slotTransform.gameObject != slotContainer)
                .Select(slotTransform => new InventorySlot
                {
                    OriginalScale = Vector3.zero,
                    Grabbable = null,
                    Transform = slotTransform
                })
                .ToList();
        }

        private void Update()
        {
            UpdateReleasedItems();
            UpdateInput();
        }
    }
}

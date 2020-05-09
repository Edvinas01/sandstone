using UnityEngine;

namespace Util
{
    public class GrabberInventory : MonoBehaviour
    {
        [Tooltip("Where the inventory items will be stored")]
        public GrabbableSnapper snapper;

        [Tooltip("Button which triggers inventory actions")]
        public OVRInput.Button trigger = OVRInput.Button.One;

        [Tooltip("Assigned grabbed to this inventory")]
        public OVRGrabber grabber;

        private void SetActiveGrabbables(bool active)
        {
            foreach (var grabbable in snapper.GetSnapped())
            {
                grabbable.gameObject.SetActive(active);
            }
        }

        private void Update()
        {
            var controller = grabber.Controller;
            if (OVRInput.GetDown(trigger, controller))
            {
                var grabbable = grabber.grabbedObject;
                if (grabbable != null && grabbable.allowInventory)
                {
                    if (snapper.Snap(grabbable))
                    {
                        grabbable.gameObject.SetActive(false);
                    }
                    return;
                }

                SetActiveGrabbables(true);
            }
            else if (OVRInput.GetUp(trigger, controller))
            {
                SetActiveGrabbables(false);
            }
        }
    }
}

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

        private void Update()
        {
            var controller = grabber.Controller;
            if (OVRInput.GetDown(trigger, controller))
            {
                var grabbable = grabber.grabbedObject;
                if (grabbable != null && grabbable.allowInventory)
                {
                    snapper.Snap(grabbable);
                    return;
                }

                snapper.SetActiveGrabbables(true);
            }
            else if (OVRInput.GetUp(trigger, controller))
            {
                snapper.SetActiveGrabbables(false);
            }
        }
    }
}

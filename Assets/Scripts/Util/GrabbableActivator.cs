using System;
using UnityEngine;

namespace Util
{
    public class GrabbableActivator : MonoBehaviour
    {
        [Tooltip("Object which must be grabbed in order to trigger the activator")]
        public OVRGrabbable grabbable;

        [Tooltip("Activator to trigger")]
        public Activator activator;

        [Tooltip("Button to trigger the activator of the grabbed object")]
        public OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;

        [Tooltip("Should the activator be toggled")]
        public bool toggle;

        public void Start()
        {
            if (grabbable == null)
            {
                grabbable = GetComponent<OVRGrabbable>();
            }

            if (activator == null)
            {
                activator = GetComponent<Activator>();
            }
        }

        public void Update()
        {
            if (toggle)
            {
                HandleToggleInput();
            }
            else
            {
                HandleInput();
            }
        }

        private bool IsInput(Func<OVRInput.Button, OVRInput.Controller, bool> input)
        {
            var grabbedBy = grabbable.grabbedBy;
            return grabbedBy != null && input(button, grabbedBy.Controller);
        }

        private bool IsDown()
        {
            return IsInput(OVRInput.GetDown);
        }

        private bool IsUp()
        {
            return IsInput(OVRInput.GetUp);
        }

        private void HandleToggleInput()
        {
            if (!IsDown())
            {
                return;
            }

            if (activator.isActive)
            {
                activator.Deactivate();
            }
            else
            {
                activator.Activate();
            }
        }

        private void HandleInput()
        {
            if (IsDown())
            {
                activator.Activate();
            }

            if (IsUp())
            {
                activator.Deactivate();
            }
        }
    }
}

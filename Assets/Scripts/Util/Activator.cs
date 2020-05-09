using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class Activator : MonoBehaviour
    {
        public enum Mode
        {
            None,
            Activate,
            Deactivate
        }

        [Tooltip("Mode to start the activator with")]
        public Mode mode = Mode.None;

        [Tooltip("Current state of the activator")]
        public bool isActive;

        [Tooltip("Event fired when the activator is activated")]
        public UnityEvent onActivated;

        [Tooltip("Event fired when the activator is deactivated")]
        public UnityEvent onDeactivated;

        private bool modeHandled;

        public void Activate()
        {
            onActivated.Invoke();
            isActive = true;
        }

        public void Deactivate()
        {
            onDeactivated.Invoke();
            isActive = false;
        }

        private void UpdateModel()
        {
            switch (mode)
            {
                case Mode.None:
                    break;
                case Mode.Activate:
                    Activate();
                    break;
                case Mode.Deactivate:
                    Deactivate();
                    break;
            }
        }

        private void OnDisable()
        {
            Deactivate();
        }

        private void Update()
        {
            if (modeHandled)
            {
                return;
            }

            UpdateModel();
            modeHandled = true;
        }
    }
}

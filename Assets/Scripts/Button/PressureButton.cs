using UnityEngine;
using UnityEngine.Events;

namespace Button
{
    public class PressureButton : MonoBehaviour
    {
        [Tooltip("The moving part of the button")]
        public Rigidbody buttonBody;

        [Tooltip("Point when which when reached by the button will trigger it")]
        public Transform activationPoint;

        [Tooltip("Distance threshold to activation point, when the button should activate")]
        public float activationThreshold = 0.1f;

        [Tooltip("Can the button be activated only once")]
        public bool singleUse;

        [Tooltip("Fired when button is activated")]
        public UnityEvent onActivated;

        [Tooltip("Fired when button is deactivated")]
        public UnityEvent onDeactivated;

        private bool active;

        private void FixedUpdate()
        {
            if (singleUse && active)
            {
                return;
            }

            var distance = Vector3.Distance(
                buttonBody.transform.position,
                activationPoint.position
            );

            if (distance <= activationThreshold && !active)
            {
                onActivated.Invoke();
                active = true;

                if (singleUse)
                {
                    buttonBody.isKinematic = true;
                }
            }
            else if (active)
            {
                onDeactivated.Invoke();
                active = false;
            }
        }
    }
}

namespace Buttons
{
    using UnityEngine;
    using UnityEngine.Events;
    
    public class PressureButton : MonoBehaviour
    {
        [Tooltip("Rigid body which is attached to the button")]
        public Rigidbody buttonBody;
    
        [Tooltip("Base the button is attached to")]
        public Transform buttonBase;

        [Tooltip("Distance threshold to button base, when the button should activate")]
        public float activationThreshold = 0.1f;
    
        [Tooltip("Can the button be activated only once")]
        public bool singleUse;

        [Tooltip("Fired when button is activated")]
        public UnityEvent activated;
    
        [Tooltip("Fired when button is deactivated")]
        public UnityEvent deactivated;
    
        private bool active;

        public void Update()
        {
            if (singleUse && active)
            {
                return;
            }

            var distance = Vector3.Distance(
                buttonBase.position,
                transform.position
            );

            if (distance <= activationThreshold)
            {
                if (!active)
                {
                    activated.Invoke();
                    active = true;

                    if (singleUse)
                    {
                        buttonBody.isKinematic = true;
                    }
                }
            }
            else
            {
                if (active)
                {
                    deactivated.Invoke();
                    active = false;
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace Velocity
{
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityDetector : MonoBehaviour
    {
        [Tooltip("Minimum velocity which registers events")]
        public float minVelocity = 0.01f;

        [Tooltip("Called when current velocity is greater than minimum")]
        public UnityEvent onVelocityEnter;

        [Tooltip("Called when current velocity is less than minimum")]
        public UnityEvent onVelocityExit;

        private bool entered;
        private Rigidbody body;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var velocity = body.velocity.magnitude;
            if (velocity <= minVelocity)
            {
                if (!entered)
                {
                    return;
                }

                onVelocityExit.Invoke();
                entered = false;
                return;
            }

            if (entered)
            {
                return;
            }

            onVelocityEnter.Invoke();
            entered = true;
        }
    }
}

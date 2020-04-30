using UnityEngine;

namespace Trap
{
    [RequireComponent(typeof(Rigidbody))]
    public class DropTrap : MonoBehaviour
    {
        private Rigidbody body;
        private bool updated;

        /// <summary>
        /// Triggers this trap and its neighbours.
        /// </summary>
        public void Trigger()
        {
            if (!body.isKinematic)
            {
                return;
            }

            body.isKinematic = false;
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }
    }
}

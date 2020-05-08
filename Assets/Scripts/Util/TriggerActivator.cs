using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerActivator : MonoBehaviour
    {
        [Tooltip("Can this activator be used only once")]
        public bool singleUse;

        [Tooltip("Fired when a collider enters the trigger")]
        public TriggerEvent onEnter;

        [Tooltip("Fired when a collider exits the trigger")]
        public TriggerEvent onExit;

        private bool used;

        [Serializable]
        public class TriggerEvent : UnityEvent<GameObject>
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (singleUse && used)
            {
                return;
            }

            onEnter.Invoke(other.gameObject);
            used = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (singleUse && used)
            {
                return;
            }

            onExit.Invoke(other.gameObject);
            used = true;
        }
    }
}

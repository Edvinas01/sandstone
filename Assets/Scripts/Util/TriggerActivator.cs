using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerActivator : MonoBehaviour
    {
        [Tooltip("Fired when a collider enters the trigger")]
        public TriggerEvent onEnter;

        [Tooltip("Fired when a collider exits the trigger")]
        public TriggerEvent onExit;

        [Serializable]
        public class TriggerEvent : UnityEvent<GameObject>
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            onEnter.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            onExit.Invoke(other.gameObject);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly HashSet<Collider> oldStaying =
            new HashSet<Collider>();

        private readonly HashSet<Collider> staying =
            new HashSet<Collider>();

        // Need to use hacky collider tracking as disabled colliders wont trigger exit.
        private void OnTriggerStay(Collider other)
        {
            staying.Add(other);
        }

        private void OnEntered(Component other)
        {
            if (singleUse && used)
            {
                return;
            }

            onEnter.Invoke(other.gameObject);
            used = true;
        }

        private void OnExited(Component other)
        {
            if (singleUse && used)
            {
                return;
            }

            onExit.Invoke(other.gameObject);
            used = true;
        }

        private void FixedUpdate()
        {
            foreach (var entered in staying.Except(oldStaying))
            {
                OnEntered(entered);
            }

            foreach (var exited in oldStaying.Except(staying))
            {
                OnExited(exited);
            }

            oldStaying.Clear();
            oldStaying.UnionWith(staying);

            staying.Clear();
        }
    }
}

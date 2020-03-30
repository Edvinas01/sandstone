using UnityEngine;
using Util;

namespace Effect
{
    [RequireComponent(typeof(Collider), typeof(Activator))]
    public class Flame : MonoBehaviour
    {
        private Activator activator;

        public void Start()
        {
            activator = GetComponent<Activator>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!activator.isActive)
            {
                return;
            }

            var otherFlame = other.GetComponent<Flame>();
            if (otherFlame == null || otherFlame.activator.isActive)
            {
                return;
            }

            otherFlame.activator.Activate();
        }
    }
}

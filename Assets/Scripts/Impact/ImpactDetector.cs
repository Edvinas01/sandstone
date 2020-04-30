using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Impact
{
    [RequireComponent(typeof(Rigidbody))]
    public class ImpactDetector : MonoBehaviour
    {
        [Tooltip("Impulse until which impact detector ignores")]
        public float minImpulse = 1;

        [Tooltip("Impulse level from which impact detector registers maximum strength")]
        public float maxImpulse = 5;

        [Tooltip("Called when impact is detected")]
        public OnHitEvent onImpact;

        [Serializable]
        public class OnHitEvent : UnityEvent<float>
        {
        }

        public void OnCollisionEnter(Collision other)
        {
            var magnitude = other.impulse.magnitude;
            if (!(minImpulse < magnitude))
            {
                return;
            }

            var normMagnitude = magnitude - minImpulse;
            var normMaxMagnitude = Mathf.Max(maxImpulse - minImpulse, 1f);
            var strength = Mathf.Min(normMagnitude / normMaxMagnitude, 1f);

            onImpact.Invoke(strength);
        }
    }
}

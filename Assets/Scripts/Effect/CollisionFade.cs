using System;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    // TODO: doesn't work when inside large objects
    [RequireComponent(
        typeof(SphereCollider),
        typeof(Rigidbody)
    )]
    public class CollisionFade : MonoBehaviour
    {
        private const float MinFadeLevel = 0f;
        private const float MaxFadeLevel = 1f;

        public OVRScreenFade ovrScreenFade;
        public float minRadius = 0.25f;

        private readonly List<Collider> trackedColliders = new List<Collider>();
        private SphereCollider sphereCollider;

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, minRadius);
        }

        public void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        public void FixedUpdate()
        {
            if (trackedColliders.Count == 0)
            {
                return;
            }

            ovrScreenFade.SetFadeLevel(GetFadeLevel());
        }

        private float GetFadeLevel()
        {
            var (minDistance, foundMinDistance) = GetMinDistance();
            var maxRadius = sphereCollider.radius;

            if (maxRadius < minDistance)
            {
                return foundMinDistance
                    ? MinFadeLevel
                    : MaxFadeLevel;
            }

            var normMinDistance = minDistance - minRadius;
            var normRadius = maxRadius - minRadius;

            return Mathf.Clamp(
                MaxFadeLevel - normMinDistance / normRadius,
                MinFadeLevel,
                MaxFadeLevel
            );
        }

        private Tuple<float, bool> GetMinDistance()
        {
            var minDistance = float.MaxValue;
            var position = transform.position;
            var radius = sphereCollider.radius;

            var foundMinDistance = false;
            foreach (var trackedCollider in trackedColliders)
            {
                var colliderPosition = trackedCollider.transform.position;
                var direction = (colliderPosition - position).normalized;

                if (Physics.Raycast(position, direction, out var hit, radius * 2))
                {
                    minDistance = Mathf.Min(
                        minDistance,
                        Vector3.Distance(position, hit.point)
                    );

                    foundMinDistance = true;
                }
            }

            return new Tuple<float, bool>(minDistance, foundMinDistance);
        }

        public void OnTriggerEnter(Collider other)
        {
            trackedColliders.Add(other);
        }

        public void OnTriggerExit(Collider other)
        {
            trackedColliders.Remove(other);
        }
    }
}

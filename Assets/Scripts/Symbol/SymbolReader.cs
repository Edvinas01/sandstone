using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Symbol
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Collider))]
    public class SymbolReader : MonoBehaviour
    {
        [Tooltip("Game object which will be used for pointing at symbols instead of lasers")]
        public GameObject hologram;

        [Tooltip("Light used to light up the hologram")]
        public new Light light;

        [Tooltip("How far to scale the hologram behind the object")]
        public float hologramScale = 0.8f;

        [Tooltip("Called when symbol decal is read")]
        public SymbolDecalEvent onSymbolDecal;

        [Tooltip("Called when no decals are found for reading")]
        public UnityEvent onNoDecal;

        [Tooltip("Called when symbol decal enters range of the reader")]
        public UnityEvent onEnterSymbolDecal;

        [Tooltip("Called when symbol decal exists range of the reader")]
        public UnityEvent onExitSymbolDecal;

        private SymbolDecal target;

        private readonly List<SymbolDecal> decals = new List<SymbolDecal>();

        [Serializable]
        public class SymbolDecalEvent : UnityEvent<string>
        {
        }

        /// <summary>
        /// Read closest symbol decal.
        /// </summary>
        public void ReadSymbolDecal()
        {
            if (decals.Count == 0)
            {
                onNoDecal.Invoke();
                return;
            }

            // Going super defensive right here!
            if (target == null)
            {
                onNoDecal.Invoke();
                return;
            }

            onSymbolDecal.Invoke(target.text);
        }

        private SymbolDecal GetClosestTarget()
        {
            var minDistance = float.MaxValue;
            var position = transform.position;

            SymbolDecal closest = null;
            foreach (var decal in decals)
            {
                var distance = Vector3.Distance(
                    position,
                    decal.transform.position
                );

                if (distance < minDistance)
                {
                    closest = decal;
                }
            }

            return closest;
        }

        private void SetActiveHologram(bool active)
        {
            if (hologram == null)
            {
                return;
            }

            hologram.SetActive(active);
        }

        private void SetActiveLight(bool active)
        {
            if (light == null)
            {
                return;
            }

            light.enabled = active;
        }

        private void UnTarget(SymbolDecal decal)
        {
            decal.ClearHighlight();
            SetActiveHologram(false);
            SetActiveLight(false);
            onExitSymbolDecal.Invoke();
        }

        private void Target(SymbolDecal decal)
        {
            decal.Highlight();
            SetActiveHologram(true);
            SetActiveLight(true);
            onEnterSymbolDecal.Invoke();
        }

        private void TargetClosestDecal()
        {
            var newTarget = GetClosestTarget();
            var oldTarget = target;

            if (newTarget == oldTarget)
            {
                return;
            }

            if (oldTarget != null)
            {
                UnTarget(oldTarget);
            }

            if (newTarget != null)
            {
                Target(newTarget);
            }

            target = newTarget;
        }

        private void UpdateHologram()
        {
            var hologramTransform = hologram.transform;

            var startPosition = hologramTransform.position;
            var endPosition = target.transform.position;

            var scale = hologramTransform.localScale;
            hologramTransform.localScale = new Vector3(
                scale.x,
                scale.y,
                Mathf.Abs(startPosition.z - endPosition.z) * hologramScale
            );

            hologramTransform.LookAt(endPosition);
        }

        private void UpdateLight()
        {
            var startPosition = transform.position;
            var endPosition = target.transform.position;

            light.transform.position = new Vector3(
                (startPosition.x + endPosition.x) / 2,
                (startPosition.y + endPosition.y) / 2,
                (startPosition.z + endPosition.z) / 2
            );
        }

        private void Awake()
        {
            SetActiveHologram(false);
            SetActiveLight(false);
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            UpdateHologram();
            UpdateLight();
        }

        private void OnTriggerEnter(Collider other)
        {
            var decal = other.GetComponent<SymbolDecal>();
            if (decal == null)
            {
                return;
            }

            decals.Add(decal);
            TargetClosestDecal();
        }

        private void OnTriggerExit(Collider other)
        {
            var decal = other.GetComponent<SymbolDecal>();
            if (decal == null)
            {
                return;
            }

            decals.Remove(decal);
            TargetClosestDecal();
        }
    }
}

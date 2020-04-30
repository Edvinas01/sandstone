using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Symbol
{
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(Collider))]
    public class SymbolReader : MonoBehaviour
    {
        [Tooltip("Color of the highlighting")]
        public Color highlightColor = Color.green;

        [Tooltip("Called when symbol decal is read")]
        public SymbolDecalEvent onSymbolDecal;

        [Tooltip("Called when no decals are found")]
        public UnityEvent onNoDecal;

        private LineRenderer lineRenderer;
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
                oldTarget.materialColorSwitcher.ResetColor();
                lineRenderer.enabled = false;
            }

            if (newTarget != null)
            {
                newTarget.materialColorSwitcher.SwitchColor(highlightColor);
                lineRenderer.enabled = true;
            }

            target = newTarget;
        }

        private void UpdateLaser()
        {
            var position = transform.position;
            var vertices = target.GetVertices();

            lineRenderer.positionCount = vertices.Count * 2;

            var laserIdx = 0;
            foreach (var vertex in vertices)
            {
                lineRenderer.SetPosition(laserIdx++, position);
                lineRenderer.SetPosition(laserIdx++, vertex);
            }
        }

        private void OnDrawGizmos()
        {
            if (target == null)
            {
                return;
            }

            foreach (var vertex in target.GetVertices())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(vertex, 0.05f);
            }
        }

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReadSymbolDecal();
            }

            UpdateLaser();
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

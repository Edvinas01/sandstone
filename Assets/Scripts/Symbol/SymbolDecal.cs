using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Symbol
{
    [RequireComponent(typeof(MaterialColorSwitcher))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Collider))]
    public class SymbolDecal : MonoBehaviour
    {
        [TextArea(5, 10)] [Tooltip("The text that has been encoded in this decal")]
        public string text;

        [Tooltip("Color switcher of the symbol")]
        public MaterialColorSwitcher materialColorSwitcher;

        [Tooltip("Mesh whose vertices to use in case outline container is not set")]
        public MeshFilter meshFilter;

        [Tooltip("Object holding transforms which form the outline")]
        public GameObject outlineContainer;

        private List<Transform> transforms = new List<Transform>();

        /// <returns>
        /// Sorted vertices (in world space).
        /// </returns>
        public List<Vector3> GetVertices()
        {
            if (transforms.Count > 0)
            {
                return transforms
                    .Select(tr => tr.position)
                    .ToList();
            }
            return meshFilter.mesh.vertices
                .Select(vertex => transform.TransformPoint(vertex))
                .ToList();
        }

        private void Start()
        {
            if (outlineContainer == null)
            {
                return;
            }

            transforms = outlineContainer
                .GetComponentsInChildren<Transform>()
                .Where(tr => tr.gameObject != gameObject)
                .ToList();
        }

    }
}

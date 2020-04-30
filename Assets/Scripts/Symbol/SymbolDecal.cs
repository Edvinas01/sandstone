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

        [Tooltip("Mesh whose vertices to use")]
        public MeshFilter meshFilter;

        /// <returns>
        /// Vertices in world space.
        /// </returns>
        public List<Vector3> GetVertices()
        {
            return meshFilter.mesh.vertices
                .Select(vertex => transform.TransformPoint(vertex))
                .ToList();
        }
    }
}

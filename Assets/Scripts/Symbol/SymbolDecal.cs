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

        /// <summary>
        /// Is this symbol currently highlighted
        /// </summary>
        public bool Highlighted { get; private set; }

        /// <summary>
        /// Clears highlighting for this symbol.
        /// </summary>
        public void ClearHighlight()
        {
            materialColorSwitcher.ResetColor();
            Highlighted = false;
        }

        /// <summary>
        /// Highlights this symbol.
        /// </summary>
        public void Highlight()
        {
            materialColorSwitcher.SwitchColor();
            Highlighted = true;
        }
    }
}

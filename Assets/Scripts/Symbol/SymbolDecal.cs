using TMPro;
using UnityEngine;

namespace Symbol
{
    [RequireComponent(typeof(Collider))]
    public class SymbolDecal : MonoBehaviour
    {
        private static readonly int FaceColor = Shader.PropertyToID("_FaceColor");

        [Tooltip("Text assigned to this decal")]
        public TMP_Text tmpText;

        [ColorUsage(true, true)]
        [Tooltip("Color used to highlight the text")]
        public Color highlightColor;

        [TextArea(5, 10)]
        [Tooltip("The text that has been encoded in this decal")]
        public string text;

        private Color originalColor;

        /// <summary>
        /// Is this symbol currently highlighted
        /// </summary>
        public bool Highlighted { get; private set; }

        /// <summary>
        /// Clears highlighting for this symbol.
        /// </summary>
        public void ClearHighlight()
        {
            if (tmpText != null)
            {
                tmpText.fontMaterial.SetColor(FaceColor, Color.black);
            }

            SetFontColor(originalColor);
            Highlighted = false;
        }

        /// <summary>
        /// Highlights this symbol.
        /// </summary>
        public void Highlight()
        {
            SetFontColor(highlightColor);
            Highlighted = true;
        }

        private void SetFontColor(Color color)
        {
            tmpText.fontMaterial.SetColor(FaceColor, color);
        }

        private void Awake()
        {
            originalColor = tmpText.fontMaterial.GetColor(FaceColor);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class MaterialColorSwitcher : MonoBehaviour
    {
        [Tooltip("Keywords which should be enabled on material instance")]
        public List<string> enableKeywords;

        [Tooltip("Mesh renderer whose material to switch")]
        public MeshRenderer meshRenderer;

        [Tooltip("Material whose color to switch, only name will be used")]
        public Material material;

        [Tooltip("Color to switch the material to")] [ColorUsage(false, true)]
        public Color color;

        [Tooltip("Name of the color field in the material")]
        public string colorField;

        private Material instanceMaterial;
        private Color initialColor;

        public void SwitchColor(Color newColor)
        {
            instanceMaterial.SetColor(colorField, newColor);
        }

        public void SwitchColor()
        {
            SwitchColor(color);
        }

        public void ResetColor()
        {
            instanceMaterial.SetColor(colorField, initialColor);
        }

        private void EnableKeywords(Material mat)
        {
            foreach (var keyword in enableKeywords)
            {
                mat.EnableKeyword(keyword);
            }
        }

        private void Start()
        {
            foreach (var rendererMaterial in meshRenderer.materials)
            {
                if (rendererMaterial.name.StartsWith(material.name))
                {
                    instanceMaterial = rendererMaterial;
                    EnableKeywords(instanceMaterial);

                    initialColor = rendererMaterial.GetColor(colorField);
                    break;
                }
            }
        }
    }
}

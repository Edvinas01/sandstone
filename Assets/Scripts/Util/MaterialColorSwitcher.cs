using UnityEngine;

namespace Util
{
    public class MaterialColorSwitcher : MonoBehaviour
    {
        [Tooltip("Mesh renderer whose material")]
        public MeshRenderer meshRenderer;

        [Tooltip("Material whose color to switch, only name will be used")]
        public Material material;

        [Tooltip("Color to switch the material to")] [ColorUsage(false, true)]
        public Color color;

        [Tooltip("Name of the color field in the material")]
        public string colorField;

        private Material instanceMaterial;
        private Color initialColor;

        public void Start()
        {
            foreach (var rendererMaterial in meshRenderer.materials)
            {
                if (rendererMaterial.name.StartsWith(material.name))
                {
                    instanceMaterial = rendererMaterial;
                    initialColor = rendererMaterial.GetColor(colorField);
                    break;
                }
            }
        }

        public void SwitchColor()
        {
            instanceMaterial.SetColor(colorField, color);
        }

        public void ResetColor()
        {
            instanceMaterial.SetColor(colorField, initialColor);
        }
    }
}

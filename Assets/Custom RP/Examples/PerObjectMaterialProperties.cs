using System;
using UnityEngine;

namespace Custom_RP.Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static int baseColorId = Shader.PropertyToID("_BaseColor");

        private static int cutoffId = Shader.PropertyToID("_Cutoff"),
            metallicId = Shader.PropertyToID("_Metallic"),
            smoothnessId = Shader.PropertyToID("_Smoothness");

        private static MaterialPropertyBlock block;

        [SerializeField] private Color baseColor = Color.white;

        [SerializeField, Range(0f, 1f)] private float cutoff = 0.5f,
            metallic = 0f,
            smoothness = 0.5f;

        private void OnValidate()
        {
            block ??= new MaterialPropertyBlock();

            block.SetColor(baseColorId, baseColor);
            block.SetFloat(cutoffId, cutoff);
            block.SetFloat(metallicId, metallic);
            block.SetFloat(smoothnessId, smoothness);
            GetComponent<Renderer>().SetPropertyBlock(block);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}
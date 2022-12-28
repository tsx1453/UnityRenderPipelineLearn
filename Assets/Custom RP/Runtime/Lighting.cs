using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    public class Lighting : BaseCommandBufferRenderingComp
    {
        private const int maxDirLightCount = 4;

        private static int dirLightColorId = Shader.PropertyToID("_DirectionalLightColors"),
            dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirections"),
            dirLightCountId = Shader.PropertyToID("_DirectionalLightCount"),
            dirLightShadowDataId = Shader.PropertyToID("_DirectionalLightShadowData");

        private static Vector4[] dirLightColors = new Vector4[maxDirLightCount];

        private static Vector4[] dirLightDirections = new Vector4[maxDirLightCount],
            dirLightShadowData = new Vector4[maxDirLightCount];

        private Shadows shadows = new Shadows();

        protected override string BufferName()
        {
            return "Lighting";
        }

        protected override void SetupInternal()
        {
            buffer.BeginSample(BufferName());
            shadows.Setup(context, cullingResults, shadowSettings);
            SetupLights();
            shadows.Render();
            buffer.EndSample(BufferName());
            ExecuteBuffer();
        }

        public override void CleanUp()
        {
            shadows.CleanUp();
        }

        void SetupLights()
        {
            NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;
            int dirLightCount = 0;
            for (int i = 0; i < visibleLights.Length; i++)
            {
                VisibleLight visibleLight = visibleLights[i];
                if (visibleLight.lightType == LightType.Directional)
                {
                    SetupDirectionalLight(dirLightCount++, ref visibleLight);
                    if (dirLightCount >= maxDirLightCount)
                    {
                        break;
                    }
                }
            }

            buffer.SetGlobalInt(dirLightCountId, dirLightCount);
            buffer.SetGlobalVectorArray(dirLightColorId, dirLightColors);
            buffer.SetGlobalVectorArray(dirLightDirectionId, dirLightDirections);
            buffer.SetGlobalVectorArray(dirLightShadowDataId, dirLightShadowData);
        }

        void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
        {
            dirLightColors[index] = visibleLight.finalColor;
            dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
            dirLightShadowData[index] = shadows.ReserveDirectionalShadows(visibleLight.light, index);
        }
    }
}
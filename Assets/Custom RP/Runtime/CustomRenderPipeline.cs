using UnityEngine;
using UnityEngine.Rendering;

namespace DefaultNamespace
{
    public class CustomRenderPipeline : RenderPipeline
    {
        private bool useDynamicBatching, useGPUInstancing;
        private CameraRenderer renderer = new CameraRenderer();
        private ShadowSettings shadowSettings;

        public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher,
            ShadowSettings shadowSettings)
        {
            this.shadowSettings = shadowSettings;
            this.useDynamicBatching = useDynamicBatching;
            this.useGPUInstancing = useGPUInstancing;
            GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
            GraphicsSettings.lightsUseLinearIntensity = true;
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                renderer.Render(context, camera, useDynamicBatching, useGPUInstancing, shadowSettings);
            }
        }
    }
}
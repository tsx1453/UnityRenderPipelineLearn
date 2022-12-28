using UnityEngine.Rendering;

namespace DefaultNamespace
{
    public abstract class BaseCommandBufferRenderingComp
    {
        protected abstract string BufferName();

        protected CommandBuffer buffer = new CommandBuffer();
        protected ScriptableRenderContext context;
        protected CullingResults cullingResults;
        protected ShadowSettings shadowSettings;

        protected BaseCommandBufferRenderingComp()
        {
            buffer.name = BufferName();
        }

        public void Setup(ScriptableRenderContext context, CullingResults cullingResults,
            ShadowSettings shadowSettings)
        {
            this.context = context;
            this.shadowSettings = shadowSettings;
            this.cullingResults = cullingResults;
            SetupInternal();
        }

        protected void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        protected abstract void SetupInternal();

        public abstract void CleanUp();
    }
}
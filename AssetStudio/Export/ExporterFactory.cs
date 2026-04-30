using AssetStudio.Export.Exporters;

namespace AssetStudio.Export
{
    /// <summary>
    /// Factory for creating and configuring exporter stacks.
    /// </summary>
    public static class ExporterFactory
    {
        /// <summary>
        /// Creates a default exporter stack with all built-in exporters.
        /// </summary>
        public static ExporterStack CreateDefaultStack()
        {
            var stack = new ExporterStack();

            // Register specific exporters
            stack.Register(new Texture2DExporter(),
                ClassIDType.Texture2D);

            stack.Register(new AudioClipExporter(),
                ClassIDType.AudioClip);

            stack.Register(new TextAssetExporter(),
                ClassIDType.TextAsset);

            stack.Register(new ShaderExporter(),
                ClassIDType.Shader);

            stack.Register(new MeshExporter(),
                ClassIDType.Mesh);

            stack.Register(new SpriteExporter(),
                ClassIDType.Sprite);

            stack.Register(new FontExporter(),
                ClassIDType.Font);

            stack.Register(new VideoClipExporter(),
                ClassIDType.VideoClip);

            // Fallback exporter for any unhandled types
            stack.RegisterFallback(new RawAssetExporter());

            return stack;
        }

        /// <summary>
        /// Creates an exporter stack with only image exporters.
        /// </summary>
        public static ExporterStack CreateImageExportStack()
        {
            var stack = new ExporterStack();
            stack.Register(new Texture2DExporter(), ClassIDType.Texture2D);
            stack.Register(new SpriteExporter(), ClassIDType.Sprite);
            return stack;
        }

        /// <summary>
        /// Creates an exporter stack with only audio exporters.
        /// </summary>
        public static ExporterStack CreateAudioExportStack()
        {
            var stack = new ExporterStack();
            stack.Register(new AudioClipExporter(), ClassIDType.AudioClip);
            return stack;
        }

        /// <summary>
        /// Creates an exporter stack with only model exporters.
        /// </summary>
        public static ExporterStack CreateModelExportStack()
        {
            var stack = new ExporterStack();
            stack.Register(new MeshExporter(), ClassIDType.Mesh);
            return stack;
        }
    }
}

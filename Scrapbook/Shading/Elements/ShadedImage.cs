using ComputeSharp;
using ComputeSharp.Resources;
using Scrapbook.Shading.Shaders;
using Scrapbook.Shading.Shaders.Processors;
using System.IO;
using System.Reflection;

namespace Scrapbook.Shading.Elements
{
    public class ShadedImage<T> : IDisposable where T : EffectProcessor
    {
        protected static GraphicsDevice Graphics => GraphicsDevice.GetDefault();
        protected readonly Stream RawDataStream;
        protected EffectProcessor Processor;
        protected readonly int Width, Height;
        public Texture2D<Rgba32>? ShadedTexture { get; protected set; } = null;
        public ReadWriteTexture2D<Rgba32, float4> RawTexture { get; private set; }

        public ShadedImage(int width, int height, Stream dataStream)
        {
            RawDataStream = dataStream;
            Width = width;
            Height = height;
            RawTexture = Graphics.LoadReadWriteTexture2D<Rgba32, float4>(RawDataStream);
            Processor = CreateProcessor();
        }

        public void Apply()
        {
            ReadWriteTexture2D<Rgba32, float4> raw = RawTexture;
            RawDataStream.Position = 0;
            Texture2D<Rgba32> finalTexture = Processor.Apply(RawDataStream);
            ShadedTexture = finalTexture;
        }

        private EffectProcessor CreateProcessor()
        {
            Type type = typeof(T);
            ConstructorInfo constructor = type.GetConstructor([]) ?? throw new Exception($"Constructor for '{type.Name}' has not been found");

            object created = constructor.Invoke([]);

            return (T) created;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

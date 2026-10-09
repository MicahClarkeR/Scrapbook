using ComputeSharp;
using ComputeSharp.Resources;
using SixLabors.ImageSharp;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using ImageSharpRgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;


namespace Scrapbook.Shading.Shaders.Processors
{
    public class GrayscaleEffectProcessor : EffectProcessor
    {
        private static GraphicsDevice Graphics => GraphicsDevice.GetDefault();

        public GrayscaleEffectProcessor() : base()
        {

        }

        public override Texture2D<Rgba32> Apply(Stream fileStream)
        {
            ReadWriteTexture2D<Rgba32, float4> texture = Graphics.LoadReadWriteTexture2D<Rgba32, float4>(fileStream);

            Graphics.For<GrayscaleEffect>(texture.Width, texture.Height, new GrayscaleEffect(texture));

            return texture;

        }
    }
}

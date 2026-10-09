using ComputeSharp;
using ComputeSharp.Resources;
using System.IO;


namespace Scrapbook.Shading.Shaders.Processors
{
    public abstract class EffectProcessor
    {
        public abstract Texture2D<Rgba32> Apply(Stream fileStream);
    }
}

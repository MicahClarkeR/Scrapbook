using ComputeSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Shading.Shaders
{
    [ThreadGroupSize(DefaultThreadGroupSizes.XY)]
    [GeneratedComputeShaderDescriptor]
    public readonly partial struct GrayscaleEffect(IReadWriteNormalizedTexture2D<float4> texture) : IComputeShader
    {
        private const int Radius = 20;

        public void Execute()
        {
            // Our image processing logic here. In this example, we are just
            // applying a naive grayscale effect to all pixels in the image.
            float3 rgb = texture[ThreadIds.XY].RGB;
            float3 avg = 1 - (Hlsl.Dot(rgb, new float3(0.0722f, 0.7152f, 0.2126f)));

            texture[ThreadIds.XY].RGB = avg;
        }


    }
}

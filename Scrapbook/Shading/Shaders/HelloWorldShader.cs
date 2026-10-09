using ComputeSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Shading.Shaders
{
    [ThreadGroupSize(DefaultThreadGroupSizes.XY)]
    [GeneratedComputeShaderDescriptor]
    internal readonly partial struct HelloWorldShader(float time) : IComputeShader<float4>
    {
        /// <inheritdoc/>
        public float4 Execute()
        {
            // Normalized screen space UV coordinates from 0.0 to 1.0
            float2 uv = ThreadIds.Normalized.XY;

            // Time varying pixel color
            float3 col = 0.5f + (0.5f * Hlsl.Cos(time + new float3(uv, uv.X) + new float3(0, 2, 4)));

            // Output to screen
            return new(col, 1f);
        }
    }
}

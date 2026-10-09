using Scrapbook.Rendering.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Rendering
{
    public sealed record ShaderEffectDefinition : EffectDefinition
    {
        public Guid ShaderAssetId { get; init; }

        public Dictionary<string, ShaderParameterValue> Parameters { get; init; } = [];
    }
}

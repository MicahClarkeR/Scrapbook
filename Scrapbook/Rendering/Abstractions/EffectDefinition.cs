namespace Scrapbook.Rendering.Abstractions
{
    /// <summary>
    /// Placeholder that will be extended upon later to add shader effects to the renderer.
    /// </summary>
    public abstract record EffectDefinition
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public bool IsEnabled { get; set; } = true;
    }
}
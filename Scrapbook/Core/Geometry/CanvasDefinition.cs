using System.Xml.Serialization;

namespace Scrapbook.Core.Geometry
{
    public sealed class CanvasDefinition
    {
        public Size2D ToSize() => new Size2D(Width, Height);
        public int Width { get; set; } = 6000;
        public int Height { get; set; } = 4000;

        [XmlElement]
        public ColourValue Background { get; set; } = Colours.White;

        public CanvasDefinition()
        {

        }
    }
}
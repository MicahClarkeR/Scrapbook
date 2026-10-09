using System.Windows.Media;

namespace Scrapbook.Core.Geometry
{
    public readonly struct Colours
    {
        public readonly static ColourValue White = new(255, 255, 255);
        public readonly static ColourValue Red = new(255, 0, 0);
        public readonly static ColourValue Green = new(0, 255, 0);
        public readonly static ColourValue Blue = new(0, 0, 255);
        public readonly static ColourValue Transparent = new(255, 255, 255, 0);
    }

    public class ColourValue
    {

        public byte Red { get; set; } = 0;
        public byte Green { get; set; } = 0;
        public byte Blue { get; set; } = 0;
        public byte Alpha { get; set; } = 0;

        public ColourValue(byte red, byte green, byte blue, byte alpha = 255)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public ColourValue()
        {
        }

        public Brush ToBrush()
        {
            return new SolidColorBrush()
            {
                Color = new Color()
                {
                    R = Red,
                    G = Green,
                    B = Blue,
                    A = Alpha
                },
                Opacity = (Alpha / 255)
            };
        }
    }
}
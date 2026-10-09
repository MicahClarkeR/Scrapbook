using System.Windows;

namespace Scrapbook.Core.Geometry
{
    public class Size2D
    {
        public static Size2D Zero => new Size2D(0, 0);

        public float Width
        {
            get => _width;
            set
            {
                if (value < 0)
                    return;

                _width = float.Round(value);
            }
        }
        private float _width = 0;
        public float Height
        {
            get => _height;
            set
            {
                if (value < 0)
                    return;

                _height = float.Round(value);
            }
        }
        private float _height = 0;

        public Size2D(float width, float height)
        {
            Width = width;
            Height = height;
        }
        public Size2D(double width, double height)
        {
            Width = (float) width;
            Height = (float) height;
        }
        public Size2D()
        {

        }

        public Size ToSize() => new Size(Width, Height);
    }
}
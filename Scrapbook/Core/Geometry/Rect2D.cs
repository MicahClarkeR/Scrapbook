using System.Numerics;

namespace Scrapbook.Core.Geometry
{
    public class Rect2D
    {
        public static Rect2D Zero => new Rect2D(0, 0, 0, 0);
        public Vector2[] Coordinates => [TopLeft, TopRight, BottomLeft, BottomRight];
        public Vector2 TopLeft => new Vector2(Left, Top);
        public Vector2 TopRight => new Vector2(Right, Top);
        public Vector2 BottomLeft => new Vector2(Left, Bottom);
        public Vector2 BottomRight => new Vector2(Right, Bottom);

        public float Left
        {
            get => _left;
            set
            {
                if (Right > value)
                    Right = value;
                else
                    _left = value;
            }
        }
        private float _left = 0;
        public float Top
        {
            get => _top;
            set
            {
                if (Bottom > value)
                    Bottom = value;
                else
                    _top = value;
            }
        }
        private float _top = 0;
        public float Right
        {
            get => _right;
            set
            {
                if (Left < value)
                    Left = value;
                else
                    _right = value;
            }
        }
        private float _right = 0;
        public float Bottom
        {
            get => _bottom;
            set
            {
                if (Top < value)
                    Top = value;
                else
                    _bottom = value;
            }
        }
        private float _bottom = 0;

        public float Width => Bottom - Top;
        public float Height => Right - Left;

        public Rect2D(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public Rect2D()
        {

        }
    }
}
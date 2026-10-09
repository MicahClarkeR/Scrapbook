using System.Numerics;

namespace Scrapbook.Core.Geometry
{
    public class Transform2D
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.One;
        public float Rotation { get; set; } = 0;
        public Vector2 Pivot { get; set; } = new Vector2(0.5f);

        public Transform2D()
        {

        }

        public Transform2D(Vector2 position)
        {
            Position = position;
        }

        public Transform2D(Vector2 position, Vector2 scale, float rotation, Vector2 pivot)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
            Pivot = pivot;
        }

        public static Transform2D Identity => new(Vector2.Zero, Vector2.One, 0, new Vector2(0.5f));
        public Transform2D Copy() => new(Position, Scale, Rotation, Pivot);
    }
}
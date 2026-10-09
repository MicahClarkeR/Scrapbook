using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Geometry.Utilities
{
    public static class ShapeUtilities
    {
        public static bool Contains(this Rect2D rect, Vector2 point)
            => rect.Left < point.X &&
               rect.Right > point.X &&
               rect.Top < point.Y &&
               rect.Bottom > point.Y;

        public static bool Contains(this Rect2D rect, Rect2D other, ContainMode mode = ContainMode.Intersect)
        {
            Vector2[] points = other.Coordinates;
            bool? result = null;

            foreach(Vector2 point in points)
            {
                bool contains = rect.Contains(point);
                if (contains)
                {
                    if (mode == ContainMode.Intersect && (!result ?? false))
                        result = true;
                    else if (mode == ContainMode.Full && result == null)
                        result = true;
                }
                else
                {
                    if (mode == ContainMode.Full && result == true)
                        result = false;
                }
                        
            }

            return result ?? false;
        }

        public enum ContainMode
        {
            Full,
            Intersect
        }
    }
}

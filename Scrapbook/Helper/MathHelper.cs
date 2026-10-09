using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Helper
{
    public static class MathHelper
    {
        public static double MinMax(double val, double min, double max) => Math.Min(max, Math.Max(min, val));
        public static float MinMax(float val, float min, float max) => Math.Min(max, Math.Max(min, val));
        public static int MinMax(int val, int min, int max) => Math.Min(max, Math.Max(min, val));

        public static Point Add(Point p, Vector2 v) => new Point(p.X + v.X, p.Y + v.Y);
    }
}

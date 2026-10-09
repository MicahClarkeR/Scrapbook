using Scrapbook.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Geometry
{
    public class Point2D
    {
        public event EventHandler<ValueChangeEventArgs>? OnChange;

        public double X
        {
            get => _x;
            set
            {
                double oldValue = _x;
                _x = value;

                ValueChanged(nameof(X), oldValue, Y);
            }
        }
        public double Y
        {
            get => _y;
            set
            {
                double oldValue = _y;
                _y = value;

                ValueChanged(nameof(Y), X, oldValue);
            }
        }

        private double _x = 0, _y = 0;

        public Point2D(Point point)
        {
            _x = point.X;
            _y = point.Y;
        }

        public Point2D(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Point2D()
        {
        }

        public void Set(double x, double y)
        {
            Point oldValues = (Point)this;
            _x = x;
            _y = y;

            ValueChanged(string.Empty, oldValues.X, oldValues.Y);
        }
        public void Set(Point point) => Set(point.X, point.Y);
        public void Set(Point2D point) => Set(point.X, point.Y);

        private void ValueChanged(string name, double oldX, double oldY)
        {
            var oldPoint = new Point(oldX, oldY);
            var point = new Point(X, Y);

            OnChange?.Invoke(this, new ValueChangeEventArgs(name, oldPoint, point));
        }

        public static implicit operator Point(Point2D point) => new Point(point.X, point.Y);
        public static implicit operator Point2D(Point point) => new Point2D(point.X, point.Y);
    }
}

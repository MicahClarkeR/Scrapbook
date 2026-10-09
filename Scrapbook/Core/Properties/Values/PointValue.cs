using Scrapbook.Core.Geometry;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Scrapbook.Core.Properties.Values
{
    public sealed class PointValue : PropertyValue<Point>
    {
        [XmlElement("X")]
        public double X
        {
            get => _x;
            set => ResetValue(value, Y);
        }
        private double _x = 0;

        [XmlElement("Y")]
        public double Y
        {
            get => _y;
            set => ResetValue(X, value);
        }
        private double _y = 0;


        public PointValue(Point value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {

        }
        public PointValue() : base(new Point(0, 0))
        {

        }

        protected override object InvokeGetValue() => new Point(X, Y);

        protected override void InvokeSetValue(object value)
        {
            if (value is Point point)
            {
                _x = point.X;
                _y = point.Y;
            }
        }

        private void ResetValue(double x, double y) => ResetValue(new Point(x, y));
        private void ResetValue(Point point) => SetValue(point);

        public static explicit operator Point(PointValue x) => x.Value;
        public static explicit operator PointValue(Point x) => new PointValue(x);
    }
}

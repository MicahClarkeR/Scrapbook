using Scrapbook.Core.Geometry;
using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Scrapbook.Core.Properties.Values
{
    [XmlRoot("Size")]
    public sealed class SizeValue : PropertyValue<Size2D>
    {
        [XmlElement("Width")]
        public float Width
        {
            get => _width;
            set => ResetValue(value, Height);
        }
        private float _width = 0;

        [XmlElement("Height")]
        public float Height
        {
            get => _height;
            set => ResetValue(Width, value);
        }
        private float _height = 0;


        public SizeValue(Size2D value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {

        }
        public SizeValue() : base(new Size2D(0, 0))
        {

        }

        protected override object InvokeGetValue() => new Size2D(Width, Height);

        protected override void InvokeSetValue(object value)
        {
            if (value is Size2D size)
            {
                _width = size.Width;
                _height = size.Height;
            }
        }

        private void ResetValue(float width, float height) => ResetValue(new Size2D(width, height));
        private void ResetValue(Size2D size) => SetValue(size);

        public static explicit operator Size2D(SizeValue x) => x.Value;
        public static explicit operator SizeValue(Size2D x) => new SizeValue(x);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Geometry
{
    public class OpacityValue
    {
        public float Value { get => _value; set => Set(value); }
        private float _value = 1;

        public OpacityValue()
        {

        }

        public OpacityValue(float opacity)
        {
            Value = opacity;
        }

        public OpacityValue(double opacity)
        {
            Value = (float) opacity;
        }

        private void Set(float value)
        {
            value = Math.Clamp(value, 0, 1);
            _value = value;
        }

        public static implicit operator OpacityValue(float opacity)
        {
            return new OpacityValue(opacity);
        }

        public static implicit operator OpacityValue(double opacity)
        {
            return new OpacityValue(opacity);
        }

        public static implicit operator float(OpacityValue opacity)
        {
            return opacity.Value;
        }

        public static implicit operator double(OpacityValue opacity)
        {
            return opacity.Value;
        }
    }
}

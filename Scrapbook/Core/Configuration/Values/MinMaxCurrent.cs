using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Scrapbook.Core.Configuration.Values
{
    /// <summary>
    /// Stores a <c>double</c> value within its set minimum and maximum range.
    /// </summary>
    public class MinMaxCurrent
    {
        /// <summary>
        /// Minimum possible value of the current value.
        /// </summary>
        public readonly double Min;
        
        /// <summary>
        /// Maximum possible value of the current value.
        /// </summary>
        public readonly double Max;

        /// <summary>
        /// Current value.
        /// </summary>
        public double Current
        {
            get => _current;
            set => _current = Math.Min(Math.Max(value, Min), Max);
        }
        private double _current;

        /// <summary>
        /// Set up this MinMaxCurrent.
        /// </summary>
        /// <param name="min">Minimum allowable value.</param>
        /// <param name="max">Maximum allowable asset.</param>
        /// <param name="current">The current value, if no value is passed then will default to minimum value.</param>
        public MinMaxCurrent(double min, double max, double? current = null)
        {
            Min = min;
            Max = max;
            Current = current ?? min;
        }
    }
}

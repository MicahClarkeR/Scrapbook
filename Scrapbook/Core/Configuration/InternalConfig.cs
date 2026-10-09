using Scrapbook.Core.Configuration.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Configuration
{
    /// <summary>
    /// Static class to hold all Scrapbook configurations such as UI configurations.
    /// </summary>
    public static class InternalConfig
    {
        /// <summary>
        /// Set the maximum and minimum width of the properties column width in main interface.
        /// </summary>
        public static MinMaxCurrent PropertyLabelColumnWidth
        {
            get => _values.PropertyLabelColumnWidth;
        }

        /// <summary>
        /// Get or set the current properties column width in the user interface.
        /// </summary>
        public static double PropertyLabelColumnWidthValue
        {
            get => _values.PropertyLabelColumnWidth.Current;
            set => _values.PropertyLabelColumnWidth.Current = value;
        }

        /// <summary>
        /// Struct to hold internal UI values.
        /// </summary>
        private static UIValues _values;

        static InternalConfig()
        {
            _values = new UIValues();
        }
    }
}

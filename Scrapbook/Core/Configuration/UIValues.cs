using Scrapbook.Core.Configuration.Values;

namespace Scrapbook.Core.Configuration
{
    /// <summary>
    /// Struct to save current UI configuration values.<br />
    /// Not implemented currently but future feature will save and load this with each session.
    /// </summary>
    public struct UIValues
    {
        /// <summary>
        /// Current minimum, maximum, and current width of the properties column in main interface.
        /// </summary>
        public MinMaxCurrent PropertyLabelColumnWidth;

        public UIValues()
        {
            PropertyLabelColumnWidth = new MinMaxCurrent(50, 2000, 150);
        }
    }
}

using Scrapbook.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements.Data
{
    public class SizedElementData : VisualElementData
    {
        /// <summary>
        /// Visual size of this element.
        /// </summary>
        public Size2D Size { get; set; } = Size2D.Zero;
    }
}

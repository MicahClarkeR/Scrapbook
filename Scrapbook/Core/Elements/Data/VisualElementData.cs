using Scrapbook.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements.Data
{
    /// <summary>
    /// Underlying class for all Elements data that will be used for saving and loading.
    /// </summary>
    public class VisualElementData : ElementData
    {
        /// <summary>
        /// Current position of the Element.
        /// </summary>
        public Point2D Position { get; set; } = new Point2D(0, 0);
        
        /// <summary>
        /// Rotation of this Element.
        /// </summary>
        public int Rotation { get; set; } = 0;

        /// <summary>
        /// Z index for this Element, used for laying.
        /// </summary>
        public int ZIndex { get; set; } = 0;

        /// <summary>
        /// Declares if this Element is visible.
        /// </summary>
        public bool IsVisisble { get; set; } = true;
    }
}

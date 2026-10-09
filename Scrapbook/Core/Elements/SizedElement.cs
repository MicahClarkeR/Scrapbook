using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements
{
    /// <summary>
    /// Generic class for any Board Element that has dimensional properties.
    /// </summary>
    public abstract class SizedElement : VisualElement
    {
        /// <summary>
        /// Size to use when displaying this Element.
        /// </summary>
        public Size2D Size { get => SizeData.Size; set => SizeData.Size = value; }

        private SizedElementData SizeData => (SizedElementData) GetData();
    }
}

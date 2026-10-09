using Scrapbook.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Elements.Data
{
    public class ImageData : SizedElementData
    {
        /// <summary>
        /// ID of the asset the image asset used by the Element using this data.
        /// </summary>
        public Guid AssetId { get; set; } = Guid.Empty;

        /// <summary>
        /// Sets if aspect ratio needs to be maintained.
        /// </summary>
        public bool MaintainAspectRatio { get; set; } = true;

        /// <summary>
        /// Size of the image on the canvas.
        /// </summary>
        public Size ImageSize { get; set; } = new Size(0, 0);
        
        /// <summary>
        /// Crop of the image on the canvas.
        /// </summary>
        public Rect2D? SourceCrop { get; set; } = null;
    }
}

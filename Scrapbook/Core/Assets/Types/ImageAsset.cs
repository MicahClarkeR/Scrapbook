using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Scrapbook.Core.Assets.Types
{
    public sealed class ImageAsset : BoardAsset
    {
        /// <summary>
        /// Width of the asset's image.
        /// </summary>
        public int PixelWidth { get; init; }

        /// <summary>
        /// Height of the asset's image.
        /// </summary>
        public int PixelHeight { get; init; }

        /// <summary>
        /// Byte data of the image.
        /// </summary>
        [XmlIgnore]
        public byte[] Content { get; set; } = [];
        
        public override byte[] GetData()
        {
            return Content;
        }
    }
}

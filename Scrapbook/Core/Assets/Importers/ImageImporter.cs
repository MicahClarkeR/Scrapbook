using Scrapbook.Core.Assets.Importers.Exceptions;
using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Scrapbook.Core.Assets.Importers
{
    /// <summary>
    /// Importer for all image assets.
    /// </summary>
    public class ImageImporter : AssetImporter<ImageAsset>
    {
        public ImageImporter()
        {
        }

        public override async Task<ImageAsset> ReadAsync(Stream source, string filename)
        {
            using var buffer = new MemoryStream();
            source.CopyTo(buffer);
            buffer.Position = 0;

            BitmapFrame frame = BitmapFrame.Create(buffer);
            byte[] content = buffer.ToArray();
            ImageMetadata metadata = frame.Metadata;
            string mime = frame.Decoder.CodecInfo.MimeTypes;

            return new ImageAsset
            {
                Name = Path.GetFileName(filename),
                MediaType = mime,
                PixelWidth = frame.PixelWidth,
                PixelHeight = frame.PixelHeight,
                Content = content
            };
        }
    }
}

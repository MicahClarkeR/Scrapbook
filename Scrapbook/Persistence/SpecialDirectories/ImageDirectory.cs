using Scrapbook.Core.Assets;
using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements;
using Scrapbook.Helper;
using Scrapbook.Persistence.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Scrapbook.Persistence.SpecialDirectories
{
    /// <summary>
    /// Represents a specialised directory for storing images into a Zip archive.
    /// </summary>
    public class ImageDirectory : ZipperDirectory
    {
        public const string DirectoryName = "images";
        public const string DataFileName = "ImageDirectoryData";

        private List<ImageElement> Images = new List<ImageElement>();

        private ImageDirectoryData _data = new ImageDirectoryData();

        public ImageDirectory(string? name = null) : base(name ?? DirectoryName)
        {

        }

        public  override void PrepareToSave()
        {
            _data.PrepareToSave();

            AddOrUpdate(_data);
        }

        public void Add(ImageElement image, ImageAsset asset)
        {
            string id = image.Id.ToString();
            byte[] assetData = asset.GetData();
            ZipperFile file = new ZipperFile(id, assetData);

            Images.Add(image);
            Add(id, file);

            _data.Images.Add(image);
            _data.Assets.Add(asset);
        }

        public class ImageDirectoryData : ZipperFile
        {
            public List<ImageElement> Images { get; set; } = new List<ImageElement>();
            public List<ImageAsset> Assets { get; set; } = new List<ImageAsset>();

            public ImageDirectoryData() : base(DataFileName, null as byte[])
            {

            }

            public override void PrepareToSave()
            {
                base.PrepareToSave();

                StringWriter writer = new StringWriter();
                XmlSerializer serializer = new XmlSerializer(typeof(ImageDirectoryData));
                
                serializer.Serialize(writer, this);

                string xml = writer.ToString();
                SetData(xml);
            }
        }

        public static void Load(BoardFile boardFile, ImageDirectory directory)
        {
            ImageDirectoryData data = directory.GetFile<ImageDirectoryData>(DataFileName);

            foreach (var image in data.Images)
            {
                ZipperFile file = directory.GetFile(image.Id);
                ImageAsset? asset = data.Assets.Where(x => x.Id == image.Id).FirstOrDefault();

                if (asset == null)
                    continue;

                asset.Content = file.Data;

                boardFile.Document.Assets.Add(asset);
            }
        }
    }
}

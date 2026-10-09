using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Scrapbook.Persistence.Files
{
    /// <summary>
    /// File used to store important metadata on the Image Asset files which are compiled into the Zip archive for the project.
    /// </summary>
    public class ImagesMetadataFile : ZipperFile
    {
        public const string Filename = "_imagemetadata";

        /// <summary>
        /// Currently stored Image Element data.
        /// </summary>
        /// <see cref="ImageData"/>
        // Stored by the Image Element's ID.
        public SerializableDictionary<Guid, ImageData> ImageElementData { get; set; } = new SerializableDictionary<Guid, ImageData>();

        /// <summary>
        /// Currently stored Image Asset data.
        /// </summary>
        /// <see cref="ImageAsset"/>
        public SerializableDictionary<Guid, ImageAsset> ImageAssetData { get; set; } = new SerializableDictionary<Guid, ImageAsset>();

        public ImagesMetadataFile() : base(Filename, null as byte[])
        {

        }

        /// <summary>
        /// Add an image to the metadata file.
        /// </summary>
        /// <param name="image">Image element display data to be stored.</param>
        /// <param name="asset">Asset associated with the image element to be stored.</param>
        public void Add(ImageElement image, ImageAsset asset)
        {
            ImageData data = image.GetData();
            ImageElementData.Add(image.Id, data);
            ImageAssetData.Add(image.Id, asset);
        }

        /// <summary>
        /// Remove the passed element from this file.
        /// </summary>
        /// <param name="image">Image Element to remove.</param>
        public void Remove(ImageElement image) => ImageElementData.Remove(image.Id);

        /// <summary>
        /// Remove an element from this file.
        /// </summary>
        /// <param name="id">ID of element to rmeove.</param>
        public void Remove(Guid id)
        {
            ImageElementData.Remove(id);
        }

        public override void PrepareToSave()
        {
            base.PrepareToSave();

            using TextWriter text = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(ImageElementAssetData));
            ImageElementAssetData data = new ImageElementAssetData(ImageElementData, ImageAssetData);

            serializer.Serialize(text, data);

            string xml = text.ToString();
            SetData(xml);
        }

        /// <summary>
        /// Deserialises currently stored Data and loads Image Asset and Element information from it.
        /// </summary>
        /// <exception cref="Exception">Thrown if no Image Element Asset information is found.</exception>
        public void LoadData()
        {
            string xml = Encoding.UTF8.GetString(Data);
            XmlSerializer serializer = new XmlSerializer(typeof(ImageElementAssetData));
            using StringReader reader = new StringReader(xml);

            object rawData = serializer.Deserialize(reader);
            ImageElementAssetData? data = (ImageElementAssetData) rawData ?? throw new Exception("Image Element Asset data returned as null.");
            ImageElementData = data.ImageElementData;
            ImageAssetData = data.ImageAssetData;
        }

        public class ImageElementAssetData
        {
            public SerializableDictionary<Guid, ImageData> ImageElementData { get; set; }
            public SerializableDictionary<Guid, ImageAsset> ImageAssetData { get; set; }

            public ImageElementAssetData()
            {
                ImageElementData = new SerializableDictionary<Guid, ImageData>();
                ImageAssetData = new SerializableDictionary<Guid, ImageAsset>();
            }
            public ImageElementAssetData(SerializableDictionary<Guid, ImageData> imageElementData, SerializableDictionary<Guid, ImageAsset> imageAssetData)
            {
                ImageElementData = imageElementData;
                ImageAssetData = imageAssetData;
            }
        }

    }
}

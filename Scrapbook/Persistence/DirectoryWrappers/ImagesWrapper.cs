using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Persistence.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using static Scrapbook.Persistence.Files.ImagesMetadataFile;

namespace Scrapbook.Persistence.DirectoryWrappers
{
    /// <summary>
    /// Images Wrapper to make adding, removing, and saving Images easier.
    /// </summary>
    internal class ImagesWrapper : ZipperDirectoryWrapper
    {
        /// <summary>
        /// Underlying images metadata.
        /// </summary>
        private ImagesMetadataFile _metadata = new ImagesMetadataFile();

        /// <inheritdoc/>
        public ImagesWrapper(ZipperDirectory directory) : base(directory)
        {

        }

        protected override void Initialise()
        {
            base.Initialise();

            if (Directory.HasFile(_metadata.Name))
            {
                ZipperFile file = Directory.GetFile(_metadata.Name);
                _metadata = new ImagesMetadataFile()
                {
                    Data = file.Data
                };
                _metadata.LoadData();
            }

            Directory.OnPreparingToSave += PreparingToSave;
        }

        /// <summary>
        /// When the Directory is being preapred to save, add the metadata file to it so it can be included in the zip file directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreparingToSave(object? sender, EventArgs e)
        {
            Directory.AddOrUpdate(_metadata);
        }

        /// <summary>
        /// Add an Image Element with the Asset to this archive.
        /// </summary>
        /// <param name="image">Image Element to save.</param>
        /// <param name="asset">Asset associated with the element.</param>
        public void Add(ImageElement image, ImageAsset asset)
        {
            string id = image.AssetId.ToString();
            byte[] assetData = asset.GetData();
            ZipperFile file = new ZipperFile(id, assetData);

            Directory.Add(id, file);
            _metadata.Add(image, asset);
        }

        /// <summary>
        /// Remove an Element and its Asset from this archive.
        /// </summary>
        /// <param name="image">Element to remove by its GUID.</param>
        /// <param name="asset">The associated asset to remove.</param>
        public void Remove(ImageElement image, ImageAsset asset)
        {
            Directory.Remove(image.Id);
            _metadata.Remove(image);
        }

        /// <summary>
        /// Gets and returns all the GUIDs of the Elements stored in this directory.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> GetGuids() => [.. _metadata.ImageElementData.Keys];

        /// <summary>
        /// Get an Image Asset by the GUID of the Element using it.
        /// </summary>
        /// <param name="id">GUID of the Element using this asset.</param>
        /// <returns>Found Image Asset.</returns>
        public ImageAsset GetAssetData(Guid id) => _metadata.ImageAssetData[id];

        /// <summary>
        /// Get an Image Element by its GUID.
        /// </summary>
        /// <param name="id">GUID of the Element.</param>
        /// <returns>Found Image Element.</returns>
        public ImageData GetElementData(Guid id) => _metadata.ImageElementData[id];
    }
}

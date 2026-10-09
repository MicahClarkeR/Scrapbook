using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scrapbook.Core.Assets.Importers
{
    /// <summary>
    /// Core interface for all Asset Importers.
    /// </summary>
    public interface IAssetImporter
    {
        Task<BoardAsset> Read(Stream source, string filename);
    }

    /// <summary>
    /// Base for all Asset Importers.
    /// </summary>
    /// <typeparam name="T">Type of asset being imported.</typeparam>
    public abstract class AssetImporter<T> : IAssetImporter where T : BoardAsset
    {

        /// <summary>
        /// Generic method to read the stream and return it as a Board Asset.
        /// </summary>
        /// <param name="source">Source stream to load.</param>
        /// <param name="filename">Desired filename.</param>
        /// <returns>Created Board Asset.</returns>
        async Task<BoardAsset> IAssetImporter.Read(Stream source, string filename)
        {
            return await ReadAsync(source, filename);
        }

        /// <summary>
        /// Generic method to read the stream and return it as the desired type.
        /// </summary>
        /// <param name="source">Source stream to load.</param>
        /// <param name="filename">Desired filename.</param>
        /// <returns>Created Board Asset as desired type.</returns>
        public abstract Task<T> ReadAsync(Stream source, string filename);
    }
}

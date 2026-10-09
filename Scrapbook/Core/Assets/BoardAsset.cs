using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Assets
{
    /// <summary>
    /// Base class for call Board Assets.
    /// </summary>
    public abstract class BoardAsset
    {
        /// <summary>
        /// Guid of this asset.
        /// </summary>
        public Guid Id { get; init; } = Guid.NewGuid();
        
        /// <summary>
        /// Filename for the asset.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Space to store underlying media type of the asset, i.e. image format.
        /// </summary>
        public string MediaType { get; init; } = string.Empty;

        /// <summary>
        /// Get the underlying data for this asset.
        /// </summary>
        /// <returns>Data to be saved to file.</returns>
        public abstract byte[] GetData();
    }
}

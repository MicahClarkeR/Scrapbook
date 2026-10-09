using System.Windows.Ink;

namespace Scrapbook.Core.Assets
{
    /// <summary>
    /// Class to contain all Board Assets being used in the application.
    /// </summary>
    public class AssetCatalogue
    {
        /// <summary>
        /// Currently loaded and available assets.
        /// </summary>
        private readonly Dictionary<Guid, BoardAsset> _assets = [];

        /// <summary>
        /// Trashed assets that will be deleted when the application is closed.
        /// </summary>
        private readonly Dictionary<Guid, BoardAsset> _trash = [];

        /// <summary>
        /// All currently loaded assets.
        /// </summary>
        public IReadOnlyCollection<BoardAsset> Assets => _assets.Values;

        /// <summary>
        /// Gets an asset by its ID.
        /// </summary>
        /// <param name="id">ID of the desired asset.</param>
        /// <returns>Asset or null if asset ID does not exist.</returns>
        public BoardAsset? Get(Guid id) => _assets.GetValueOrDefault(id);

        /// <summary>
        /// Gets an asset by its ID and casts it into the desired type.
        /// </summary>
        /// <typeparam name="T">Type to return asset as.</typeparam>
        /// <param name="id">ID of desired asset.</param>
        /// <returns>Asset as desired type or null of not found.</returns>
        public T? Get<T>(Guid id) where T : BoardAsset => _assets.GetValueOrDefault(id) as T;

        /// <summary>
        /// Gets all assets of given type.
        /// </summary>
        /// <typeparam name="T">Type of assets to get.</typeparam>
        /// <returns>All found assets of passed type.</returns>
        public List<T> Get<T>() where T : BoardAsset
        {
            List<T> list = new List<T>();

            foreach(var item in _assets.Values)
            {
                if (item is T tItem)
                {
                    list.Add(tItem);
                }
            }

            return list;
        }

        /// <summary>
        /// Add passed asset to the catalogue.
        /// </summary>
        /// <param name="asset">Asset to add.</param>
        public void Add(BoardAsset asset) => _assets.Add(asset.Id, asset);

        /// <summary>
        /// Permanently deletes asset by its ID.
        /// </summary>
        /// <param name="id">ID of asset to remove.</param>
        /// <returns>If the asset was found and removed.</returns>
        public bool Remove(Guid id) => _assets.Remove(id);

        /// <summary>
        /// Retore an asset from the temporary trash.
        /// </summary>
        /// <param name="id">ID of asset to restore.</param>
        /// <returns>If the asset was found and restored.</returns>
        public bool Restore(Guid id)
        {
            if (!_trash.TryGetValue(id, out BoardAsset? asset) || asset == null)
                return false;

            _trash.Remove(id);
            Add(asset);

            return true;
        }

        /// <summary>
        /// Takes an asset from the available assets and trashes it, which can be restored in this session.
        /// </summary>
        /// <param name="id">ID of asset to trash.</param>
        /// <returns>If the asset was found and trashed.</returns>
        public bool Trash(Guid id)
        {
            if (!_assets.TryGetValue(id, out BoardAsset? asset) || asset == null)
                return false;

            _assets.Remove(id);
            _trash.Add(id, asset);

            return true;
        }

        /// <summary>
        /// Delete an asset from the trash by ID.
        /// </summary>
        /// <param name="id">ID of trashed asset to delete.</param>
        /// <returns>If asset was successfully found and deleted.</returns>
        public bool DeleteTrash(Guid id) => _trash.Remove(id);
    }
}
using System.Text;

namespace Scrapbook.Persistence
{
    /// <summary>
    /// Base file to be packed into a zip archive.
    /// </summary>
    public class ZipperFile : IZipperContent
    {
        /// <summary>
        /// Current file data.
        /// </summary>
        public byte[] Data { get; set; } = new byte[0];
        
        /// <summary>
        /// Filename.
        /// </summary>
        public string Name => _name;
        private readonly string _name;

        /// <summary>
        /// Create Zipper File with passed data, name will be set to a GUID.
        /// </summary>
        /// <param name="data">Data to initalise with.</param>
        public ZipperFile(byte[]? data = null)
        {
            _name = Guid.NewGuid().ToString();

            if (data != null)
                SetData(data);
        }

        /// <summary>
        /// Create Zipper file with given filename and data.
        /// </summary>
        /// <param name="name">Desired filename.</param>
        /// <param name="data">Data for file.</param>
        public ZipperFile(string name, byte[]? data = null)
        {
            _name = name;

            if (data != null)
                SetData(data);
        }

        /// <summary>
        /// Create Zipper file with given filename and data.
        /// </summary>
        /// <param name="name">Desired filename.</param>
        /// <param name="content">Text content to be stored in file.</param>
        public ZipperFile(string name, string? content = null)
        {
            _name = name;

            if (content != null)
                SetData(content);
        }

        /// <summary>
        /// Set file data.
        /// </summary>
        /// <param name="data">Bytes to store within this file.</param>
        public void SetData(byte[] data) => Data = data;

        /// <summary>
        /// Converts passed string into bytes and stores them.
        /// </summary>
        /// <param name="content">Characters to store.</param>
        public void SetData(string content) => Data = Encoding.Default.GetBytes(content);

        /// <summary>
        /// Prepares this file for saving, such as serialising data for storage, to be called before save occures.
        /// </summary>
        public virtual void PrepareToSave()
        {

        }
    }
}

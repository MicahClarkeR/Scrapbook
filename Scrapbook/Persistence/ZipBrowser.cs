using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Scrapbook.Persistence
{
    public sealed class ZipBrowser
    {
        public readonly string FilePath;
        public readonly ZipperDirectory Root;

        public ZipBrowser(string path) : base()
        {
            FilePath = path;
            Root = new ZipperDirectory(string.Empty);

            Open();
        }

        private void Open()
        {
            using FileStream stream = new FileStream(FilePath, FileMode.Open);
            using ZipArchive archive = new ZipArchive(stream);

            foreach(var entry in archive.Entries)
            {
                string path = entry.FullName;
                string[] directories = [.. path.Split('\\').SkipLast(1)];
                string filename = Path.GetFileName(path);
                ZipperDirectory directory = (directories.Length == 0) ? Root : Root.CreateSubDirectory(directories);
                using Stream entryStream = entry.Open();
                using MemoryStream dataStream = new MemoryStream();

                entryStream.CopyTo(dataStream);
                directory.Add(filename, new ZipperFile(filename, dataStream.ToArray()));

                entryStream.Dispose();
                dataStream.Dispose();
            }
        }


    }
}

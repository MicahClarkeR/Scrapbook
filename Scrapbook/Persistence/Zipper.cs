using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text.Unicode;
using System.Xml.Linq;

namespace Scrapbook.Persistence
{
    public class Zipper
    {
        public readonly ZipperDirectory Root;

        public Zipper()
        {
            Root = new ZipperDirectory(string.Empty);
        }

        public void Add(string key, IZipperContent content) => Root.Add(key, content);

        public ZipperDirectory CreateDirectory(string name)
        {
            if (Root.TryGetDirectory(name, out ZipperDirectory? found) && found != null)
            {
                return found;
            }

            ZipperDirectory directory = new ZipperDirectory(name);
            Add(name, directory);

            return directory;
        }

        public static T CreateDirectory<T>(string? name = null) where T : ZipperDirectory
        {
            Type type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors();

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();

                if (parameters.Length == 0 && name == null)
                {
                    object created = constructor.Invoke(null);
                    T directory = (T) created;

                    return directory;
                }
                else if (parameters.Length == 1)
                {
                    object created = constructor.Invoke([name]);
                    T directory = (T) created;

                    return directory;
                }
            }

            throw new Exception("Unable to create ZipperDirectory");
        }

        public void Zip(string path)
        {
            using FileStream stream = new FileStream(path, FileMode.Create);
            ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create);

            ProcessDirectory(Root, archive, string.Empty);

            archive.Dispose();
        }

        private void AddZipperContent(IZipperContent content, ZipArchive archive, string path)
        {
            if (content is ZipperDirectory directory)
                ProcessDirectory(directory, archive, path);
            else if (content is ZipperFile file)
                ProcessFile(file, archive, path);
        }

        private void ProcessDirectory(ZipperDirectory directory, ZipArchive archive, string path)
        {
            var content = directory.Content;
            path = Path.Combine(path, directory.Name);

            foreach (string key in content.Keys)
            {
                IZipperContent value = content[key];
                AddZipperContent(value, archive, path);
            }
        }

        private static void ProcessFile(ZipperFile file, ZipArchive archive, string path)
        {
            string filePath = Path.Combine(path, file.Name);
            ZipArchiveEntry entry = archive.CreateEntry(filePath);
            using Stream stream = entry.Open();

            stream.Write(file.Data, 0, file.Data.Length);
            stream.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook2.Tests.Assets
{
    public static class AssetHelper
    {
        public static string Path => GlobalHelper.GetRootPath("Assets");

        public static string GetAssetPath(params string[] subPaths)
        {
            string subPath = System.IO.Path.Join(subPaths);
            string completePath = System.IO.Path.Join([Path, subPath]);

            return completePath;
        }
        public static FileInfo GetAssetInfo(params string[] subPaths)
        {
            string assetPath = GetAssetPath(subPaths);

            if (!File.Exists(assetPath))
                throw new Exception($"Asset file '{assetPath}' cannot be found.");

            FileInfo fileInto = new FileInfo(assetPath);
            return fileInto;
        }

        public static byte[] ReadAssetData(params string[] subPaths)
        {
            FileInfo file = GetAssetInfo(subPaths);
            byte[] bytes = File.ReadAllBytes(file.FullName);
            
            return bytes;
        }

        public static string ReadAssetText(params string[] subPaths)
        {
            FileInfo file = GetAssetInfo(subPaths);
            string contents = File.ReadAllText(file.FullName);

            return contents;
        }

        public static Stream GetFileStream(FileMode mode = FileMode.Open, params string[] subPaths)
        {
            FileInfo file = GetAssetInfo(subPaths);
            return file.Open(mode);
        }
    }
}

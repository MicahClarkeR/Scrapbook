using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scrapbook2.Tests
{
    internal static class GlobalHelper
    {
        public static string GetRootPath()
        {
            Assembly executable = Assembly.GetExecutingAssembly();
            DirectoryInfo path = Directory.GetParent(executable.Location)
                ?? throw new Exception("Unable to find executing directory path.");

            return path.FullName;
        }

        public static string GetRootPath(params string[] subPaths)
        {
            string path = GetRootPath();

            foreach (string subPath in subPaths)
            {
                path = Path.Combine(path, subPath);

                if (!Path.Exists(path))
                    throw new Exception($"Path '{path}' does not exist.");
            }

            return path;
        }
    }
}

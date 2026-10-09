using Scrapbook.Core.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core
{
    public static class GlobalDefaults
    {
        public static class UI
        {
            public readonly static string WindowTitle = "Scrapbook";
            public static int GridElementHeight => 18;
            public static float MinimumElementWidth => 50;
            public static float MinimumElementHeight => 50;
        }
    }
}

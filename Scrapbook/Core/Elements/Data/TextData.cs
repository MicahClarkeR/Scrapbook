using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements.Data
{
    public class TextData : SizedElementData
    {
        public string Text { get; set; } = string.Empty;
        public TextStyle Style { get; set; } = new();
    }
}

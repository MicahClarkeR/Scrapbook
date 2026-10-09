using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using Scrapbook.Core.Properties;
using Scrapbook.Core.Properties.Values;
using Scrapbook.Editing;
using Scrapbook.Editing.Services;
using Scrapbook.UI.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Elements
{
    /// <summary>
    /// Currently unimplemented. Used to add text to the Board Canvas.
    /// </summary>
    public sealed class TextElement : SizedElement
    {

        private TextData _data = new TextData();

        public TextElement(TextData data)
        {
            _data = data;
        }

        public TextElement()
        {
            Size = new(200, 50);
        }

        public override TextData GetData() => _data;

        protected override PropertyDictionary? GetElementProperties(BoardEditorService editor)
        {
            return null;
        }

        public override bool AddToCanvas(BoardCanvas canvas, BoardEditorSession session)
        {
            throw new NotImplementedException();
        }

        public override FrameworkElement? GetElement()
        {
            throw new NotImplementedException();
        }
    }
}

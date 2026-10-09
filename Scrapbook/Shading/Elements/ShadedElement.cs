using ComputeSharp;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Properties;
using Scrapbook.Editing;
using Scrapbook.Editing.Services;
using Scrapbook.UI.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Scrapbook.Shading.Elements
{
    public abstract class ShadedElement : VisualElement
    {
        protected GraphicsDevice Device => GraphicsDevice.GetDefault();

        public override bool AddToCanvas(BoardCanvas canvas, BoardEditorSession session)
        {
            throw new NotImplementedException();
        }

        public override IElementData GetData()
        {
            throw new NotImplementedException();
        }

        public override FrameworkElement? GetElement()
        {
            throw new NotImplementedException();
        }

        protected override PropertyDictionary? GetElementProperties(BoardEditorService service)
        {
            throw new NotImplementedException();
        }

        protected abstract void Render(ShaderConfiguration configuration);
    }
}

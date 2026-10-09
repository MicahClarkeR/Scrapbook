using Scrapbook.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Board.Operations.Elements.Models
{
    public class ElementsBoardOperationModel : IBoardOperationModel
    {
        public VisualElement[] Elements;

        public ElementsBoardOperationModel(params VisualElement[] elements)
        {
            Elements = elements;
        }
    }
}

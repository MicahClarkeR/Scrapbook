using Scrapbook.Core.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Board.Operations.Elements.Models
{
    public class SizedElementsBoardOperationModel : IBoardOperationModel
    {
        public SizedElement[] Elements;

        public SizedElementsBoardOperationModel(params SizedElement[] elements)
        {
            Elements = elements;
        }
    }
}

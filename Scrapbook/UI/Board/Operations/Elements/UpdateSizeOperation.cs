using Scrapbook.Core.Elements;
using Scrapbook.Editing;
using Scrapbook.UI.Board.Operations.Elements.Models;
using System.Windows;

namespace Scrapbook.UI.Board.Operations.Elements
{
    public class UpdateSizeOperation : BoardOperation<SizedElementsBoardOperationModel>
    {
        public override bool Execute(BoardEditorSession session, SizedElementsBoardOperationModel model)
        {
            foreach (SizedElement element in model.Elements)
            {
                FrameworkElement? visual = element.GetElement();

                if (visual != null)
                {
                    visual.Width = element.Size.Width;
                    visual.Height = element.Size.Height;
                }
            }
            return true;
        }
    }
}

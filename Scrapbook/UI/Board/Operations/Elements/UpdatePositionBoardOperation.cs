using Scrapbook.Editing;
using Scrapbook.UI.Board.Operations.Elements.Models;
using System.Windows;
using System.Windows.Controls;

namespace Scrapbook.UI.Board.Operations.Elements
{
    public class UpdatePositionBoardOperation : BoardOperation<ElementsBoardOperationModel>
    {
        public override bool Execute(BoardEditorSession session, ElementsBoardOperationModel model)
        {
            foreach(var element in model.Elements)
            {
                Point position = element.DisplayPosition;
                FrameworkElement? uiElement = element.GetElement();

                if (uiElement == null)
                    continue;

                Canvas.SetLeft(uiElement, position.X);
                Canvas.SetTop(uiElement, position.Y);
            }

            return true;
        }
    }
}

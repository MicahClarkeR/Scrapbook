using Scrapbook.Core.Elements;
using Scrapbook.Editing;
using Scrapbook.Helper;
using Scrapbook.UI.Board.Operations.Elements.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Scrapbook.UI.Board.Operations.Elements
{
    public class UpdateRotationOperation : BoardOperation<ElementsBoardOperationModel>
    {
        public override bool Execute(BoardEditorSession session, ElementsBoardOperationModel model)
        {
            foreach (VisualElement visual in model.Elements)
            {
                FrameworkElement? element = visual.GetElement();

                if (element == null)
                    continue;

                RotateTransform? existingRotation = TransformHelper.GetLayoutTransform<RotateTransform>(element);

                if (existingRotation != null && existingRotation.Angle == visual.Rotation)
                {
                    continue;
                }

                RotateTransform rotate = new RotateTransform(visual.Rotation);
                TransformHelper.AddLayoutTransform(element, rotate);
            }

            return true;
        }
    }
}

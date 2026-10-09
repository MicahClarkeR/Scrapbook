using Scrapbook.Core;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Editing.Operations.Element
{
    public class SetElementSizeOperation(Size2D size, Guid id) : ValueReplacementOperation<Size2D>(size, id)
    {
        public override string Description => "Set the Transform of an Element with the given Id.";

        public override async Task Execute(BoardDocument document)
        {
            SizedElement? element = document.Elements.Get<SizedElement>(Id);

            if (element == null)
                return;

            OldValue = element.Size;

            double newWidth = NewValue.Width;
            double newHeight = NewValue.Height;

            if (newWidth < GlobalDefaults.UI.MinimumElementWidth && newWidth < newHeight)
            {
                double ratio = newHeight / newWidth;
                double change = GlobalDefaults.UI.MinimumElementWidth - newWidth;
                newWidth += change;

                if (element is ImageElement image && image.MaintainAspectRatio)
                    newHeight += change * ratio;
            }
            else if (newHeight < GlobalDefaults.UI.MinimumElementHeight && newHeight < newWidth)
            {
                double ratio = newWidth / newHeight;
                double change = GlobalDefaults.UI.MinimumElementHeight - newHeight;
                newHeight += change;

                if (element is ImageElement image && image.MaintainAspectRatio)
                    newWidth += Math.Round(change * ratio);
            }

            element.Size = new Size2D(newWidth, newHeight);
        }

        public override async Task Undo(BoardDocument document)
        {
            SizedElement? element = document.Elements.Get<SizedElement>(Id);

            if (element == null || OldValue == default)
                return;

            element.Size = OldValue;
        }
    }
}

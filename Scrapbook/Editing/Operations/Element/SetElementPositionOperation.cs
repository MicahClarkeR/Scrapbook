using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Editing.Operations.Element
{
    public class SetElementPositionOperation(Point position, Guid id) : ValueReplacementOperation<Point>(position, id)
    {
        public override string Description => "Set the Transform of an Element with the given Id.";

        public override async Task Execute(BoardDocument document)
        {
            var element = document.Elements.Get<VisualElement>(id);

            if (element == null)
                return;

            OldValue = element.Position;
            element.Position = NewValue;
        }

        public override async Task Undo(BoardDocument document)
        {
            var element = document.Elements.Get<VisualElement>(id);

            if (element == null || OldValue == default)
                return;

            element.Position = OldValue;
        }
    }
}

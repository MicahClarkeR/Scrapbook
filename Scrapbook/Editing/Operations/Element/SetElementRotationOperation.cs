using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Scrapbook.Editing.Operations.Element
{
    public class SetElementRotationOperation(int rotation, Guid id) : ValueReplacementOperation<int>(rotation, id)
    {
        public override string Description => "Set the Transform of an Element with the given Id.";

        public override async Task Execute(BoardDocument document)
        {
            var element = document.Elements.Get<ImageElement>(id);

            if (element == null)
                return;

            OldValue = element.Rotation;
            element.Rotation = NewValue;
        }

        public override async Task Undo(BoardDocument document)
        {
            var element = document.Elements.Get<VisualElement>(id);

            if (element == null || OldValue == default)
                return;

            element.Rotation = OldValue;
        }
    }
}

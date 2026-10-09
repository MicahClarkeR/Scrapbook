using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Element
{
    [Obsolete]
    public class RenameElementOperation(string name, Guid id) : ValueReplacementOperation<string>(name, id)
    {
        public override string Description => "Rename an Element with the given Id to the given name.";

        public override async Task Execute(BoardDocument document)
        {
            BoardElement? element = document.Elements.Get(Id);

            if (element == null)
                return;

            //element.Name = NewValue;
        }

        public override async Task Undo(BoardDocument document)
        {
            BoardElement? element = document.Elements.Get(Id);

            if (element == null || OldValue == null)
                return;

            //element.Name = OldValue;
        }
    }
}

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
    public class SetElementLockedOperation : MultiElementBoardOperation<bool>
    {
        public override string Description => "Set the opacity of Element(s) with the given Id.";

        public override async Task Execute(BoardDocument document)
        {
            Reset();

            foreach (Guid id in Ids)
            {
                BoardElement? element = document.Elements.Get(id);

                if (element != null)
                {
                    //StoreValue(id, element.IsLocked);
                    //element.IsLocked = Value;
                }
            }
        }

        public override async Task Undo(BoardDocument document)
        {
            foreach(Guid id in Ids)
            {
                bool? locked = GetValue(id);

                if (locked != null)
                {
                    BoardElement? element = document.Elements.Get(id);
                    //element?.IsLocked = (bool) locked;
                }
            }
        }

        public SetElementLockedOperation(bool locked, params Guid[] ids) : base(locked, Events.DocumentChangeEvent.ElementChanged, ids) { }
        public SetElementLockedOperation(bool locked, Guid id) : base(locked, Events.DocumentChangeEvent.ElementChanged, id) { }
    }
}

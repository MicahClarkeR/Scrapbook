using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Element
{
    [Obsolete]
    public class SetElementOpacityOperation : MultiElementBoardOperation<OpacityValue>
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
                    //StoreValue(id, element.Opacity);
                    //element.Opacity = Value;
                }
            }
        }

        public override async Task Undo(BoardDocument document)
        {
            foreach(Guid id in Ids)
            {
                OpacityValue? opacity = GetValue(id);

                if (opacity == null)
                    continue;

                BoardElement? element = document.Elements.Get(id);
                //element?.Opacity = opacity;
            }
        }

        public SetElementOpacityOperation(OpacityValue opacity, params Guid[] ids) : base(opacity, Events.DocumentChangeEvent.ElementChanged, ids) { }
        public SetElementOpacityOperation(OpacityValue opacity, Guid id) : base(opacity, Events.DocumentChangeEvent.ElementChanged, id) { }
    }
}

using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Element
{
    public class SetImageAspectRatioOperation : MultiElementBoardOperation<bool>
    {
        public override string Description => "Set Image(s) to maintain aspect ratio.";

        public override async Task Execute(BoardDocument document)
        {
            Reset();

            foreach (Guid id in Ids)
            {
                var element = document.Elements.Get<ImageElement>(id);

                if (element != null)
                {
                    StoreValue(id, element.MaintainAspectRatio);
                    element.MaintainAspectRatio = Value;
                }
            }
        }

        public override async Task Undo(BoardDocument document)
        {
            foreach(Guid id in Ids)
            {
                bool? ratio = GetValue(id);

                if (ratio != null)
                {
                    var element = document.Elements.Get<ImageElement>(id);
                    element?.MaintainAspectRatio = (bool) ratio;
                }
            }
        }

        public SetImageAspectRatioOperation(bool aspect, params Guid[] ids) : base(aspect, Events.DocumentChangeEvent.ElementChanged, ids) { }
        public SetImageAspectRatioOperation(bool aspect, Guid id) : base(aspect, Events.DocumentChangeEvent.ElementChanged, id) { }
    }
}

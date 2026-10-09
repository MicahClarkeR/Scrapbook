using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Element
{
    public class SetElementVisibilityOperation : MultiElementBoardOperation<bool>
    {
        public override string Description => "Set the visibility of Element(s) with the given Id.";
        private readonly Dictionary<Guid, bool> _oldElementValues = [];

        public SetElementVisibilityOperation(bool value, Guid id) : base(value, Events.DocumentChangeEvent.ElementChanged, id)
        {

        }

        public override async Task Execute(BoardDocument document)
        {
            _oldElementValues.Clear();

            foreach (Guid id in Ids)
            {
                var element = document.Elements.Get<VisualElement>(id);

                if (element != null)
                {
                    _oldElementValues.Add(id, element.IsVisisble);
                    StoreValue(id, element.IsVisisble);

                    element.IsVisisble = Value;
                }
            }
        }

        public override async Task Undo(BoardDocument document)
        {
            foreach(KeyValuePair<Guid, bool> elValue in _oldElementValues)
            {
                Guid id = elValue.Key;
                bool visible = elValue.Value;

                var element = document.Elements.Get<VisualElement>(id);
                element?.IsVisisble = Value;
            }
        }

        public SetElementVisibilityOperation(bool visible, params Guid[] ids) : base(visible, Events.DocumentChangeEvent.ElementChanged, ids) {}
    }
}

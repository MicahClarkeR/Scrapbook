using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Events
{
    public enum DocumentChangeEvent
    {
        ElementAdded,
        ElementRemoved,
        ElementChanged,
        ElementReordered,
        AssetChanged,
        EffectChanged,
        DocumentReset
    }

    public record DocumentChangeEventArgs
    {
        public readonly DocumentChangeEvent Kind;
        public readonly Guid? AssetId = null;
        public readonly Guid[] ElementIds = [];

        public DocumentChangeEventArgs(DocumentChangeEvent kind, Guid? assetId = null, Guid? elementId = null)
        {
            Kind = kind;
            AssetId = assetId;

            if (elementId != null)
            {
                Guid[] guids = new Guid[1];
                guids[0] = (Guid) elementId;

                ElementIds = guids;
            }
        }

        public DocumentChangeEventArgs(DocumentChangeEvent kind, params Guid[] elementIds)
        {
            Kind = kind;
            ElementIds = elementIds;
        }
    }
}

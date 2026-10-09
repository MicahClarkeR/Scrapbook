using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Interfaces
{
    public abstract class MultiElementBoardOperation<T> : BoardOperation
    {
        public readonly T Value;
        public readonly Guid[] Ids;

        private readonly Dictionary<Guid, T> _oldValues = new Dictionary<Guid, T>();

        public MultiElementBoardOperation(T value, DocumentChangeEvent eventType, params Guid[] ids) : base(eventType)
        {
            Value = value;
            Ids = ids;
        }

        public MultiElementBoardOperation(T value, DocumentChangeEvent eventType, Guid id) : base(eventType)
        {
            Value = value;
            Ids = [id];
        }

        protected void StoreValue(Guid guid, T value)
        {
            _oldValues[guid] = value;
        }

        protected T? GetValue(Guid id) => _oldValues[id];
        protected bool HasValue(Guid id) => _oldValues.ContainsKey(id);
        protected Guid[] GetGuids() => [.. _oldValues.Keys];
        protected void Reset()
        {
            _oldValues.Clear();
        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, Ids);
    }
}

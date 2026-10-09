using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Interfaces
{
    public abstract class ValueReplacementOperation<T> : BoardOperation, IBoardOperation
    {
        public Guid Id { get; private set; }
        public T NewValue { get; private set; }
        public T? OldValue { get; set; } = default;

        public ValueReplacementOperation(T newValue, Guid id, DocumentChangeEvent eventType = DocumentChangeEvent.ElementChanged) : base(eventType) 
        {
            Id = id;
            NewValue = newValue;
        }
        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, Id);
    }
}

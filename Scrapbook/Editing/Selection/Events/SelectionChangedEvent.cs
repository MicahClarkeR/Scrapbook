using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Selection.Events
{
    public enum SelectionEvent
    {
        Clear,
        Addition,
        Removal,
        Replace
    }

    public class SelectionChangedEventArgs : EventArgs
    {
        public SelectionEvent EventType { get; }
        public IReadOnlyList<Guid> Additions { get; }
        public IReadOnlyList<Guid> Removals { get; }

        public SelectionChangedEventArgs(
            SelectionEvent eventType,
            IReadOnlyList<Guid> additions,
            IReadOnlyList<Guid> removals)
        {
            EventType = eventType;
            Additions = additions;
            Removals = removals;
        }
    }
}

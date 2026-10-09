using Scrapbook.Editing.Selection.Events;

namespace Scrapbook.Editing.Selection
{
    public sealed class SelectionState
    {
        private readonly HashSet<Guid> _selectedIds = [];
        public IReadOnlySet<Guid> SelectedIds => _selectedIds;
        public Guid? PrimaryId { get => (SelectedIds.Count == 1) ? SelectedIds.First() : null; }

        public event EventHandler<SelectionChangedEventArgs>? Changed;

        public bool Replace(Guid oldElement, Guid newElement, bool addRegardless = false)
        {
            if (newElement == oldElement)
                return false;

            List<Guid> additions = new();
            List<Guid> removals = new();

            if (SelectedIds.Contains(oldElement))
            {
                _selectedIds.Remove(oldElement);
                removals.Add(oldElement);

                _selectedIds.Add(newElement);
                additions.Add(newElement);

            }
            else if (addRegardless)
            {
                _selectedIds.Add(newElement);
                additions.Add(newElement);
            }

            if (additions.Count > 0)
            {
                SelectionChangedEventArgs args = new((removals.Count > 0) ? SelectionEvent.Replace : SelectionEvent.Addition, [.. additions], [.. removals]);
                OnChanged(args);

                return true;
            }

            return false;
        }
        public bool Add(params Guid[] elementIds)
        {
            List<Guid> additions = new();

            foreach (Guid elementId in elementIds)
                if (_selectedIds.Add(elementId))
                    additions.Add(elementId);

            if (additions.Count > 0)
            {
                SelectionChangedEventArgs args = new(SelectionEvent.Addition, [.. additions], []);
                OnChanged(args);

                return true;
            }

            return false;
        }
        public bool Remove(params Guid[] elementIds)
        {
            List<Guid> removals = new();

            foreach (Guid elementId in elementIds)
                if (_selectedIds.Remove(elementId))
                    removals.Add(elementId);

            if (removals.Count > 0)
            {
                SelectionChangedEventArgs args = new(SelectionEvent.Removal, [], [.. removals]);
                OnChanged(args);

                return true;
            }

            return false;
        }
        public void Clear()
        {
            SelectionChangedEventArgs args = new(SelectionEvent.Clear, [], [.. SelectedIds]);
            _selectedIds.Clear();

            OnChanged(args);
        }

        private void OnChanged(SelectionChangedEventArgs args)
        {
            Changed?.Invoke(this, args);
        }
    }
}
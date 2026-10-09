using Scrapbook.Core.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Documents
{
    /// <summary>
    /// Collection of all the currently loaded Board Elements.
    /// </summary>
    public class BoardElements : ICollection<BoardElement>
    {
        /// <summary>
        /// Count of available elements.
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// This collection is not readonly.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Internal dictionary of Board Elements stored by type and then GUID.
        /// </summary>
        private Dictionary<Type, Dictionary<Guid, BoardElement>> _elements = new Dictionary<Type, Dictionary<Guid, BoardElement>>();

        /// <summary>
        /// Cache of Board Elements created by <see cref="ToList"/>, reset to <c>null</c> when a Board Element is added or removed.
        /// </summary>
        private List<BoardElement>? _listCache = null;

        public BoardElements()
        {

        }

        /// <summary>
        /// Gets all Board Elements of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>All found Board Elements of the given type.</returns>
        public List<T> GetAll<T>() where T : BoardElement
        {
            Type type = typeof(T);

            if (!_elements.ContainsKey(type))
                return [];

            return [.. _elements[type].Values.Cast<T>()];
        }

        /// <summary>
        /// Gets and returns an element of given ID.
        /// </summary>
        /// <typeparam name="T">Type to cast found element to.</typeparam>
        /// <param name="id">ID of element to find.</param>
        /// <returns>Found element as desired type, if not found returns <c>null</c>.</returns>
        public T? Get<T>(Guid id) where T : BoardElement
        {
            BoardElement? element = Get(id);

            if (element != null)
                return (T) element;

            return null;
        }

        /// <summary>
        /// Gets and returns an element of given ID.
        /// </summary>
        /// <typeparam name="T">Type to cast found element to.</typeparam>
        /// <returns>Found element, if not found returns <c>null</c>.</returns>
        public BoardElement? Get(Guid id)
        {
            foreach (Type type in _elements.Keys)
            {
                if (_elements[type].ContainsKey(id))
                    return _elements[type][id];
            }

            return null;
        }

        /// <summary>
        /// Add passed Board Element, if it already exists it will be overwritten.
        /// </summary>
        /// <param name="element">Element to store.</param>
        public void Add(BoardElement element)
        {
            Type key = element.GetType();
            Guid id = element.Id;

            if (!_elements.ContainsKey(key))
                _elements.Add(key, new Dictionary<Guid, BoardElement>());

            var container = _elements[key];

            if (container.ContainsKey(id))
                container[id] = element;
            else
                container.Add(id, element);
            _listCache = null;

        }

        /// <summary>
        /// Clear stored Board Elements.
        /// </summary>
        public void Clear()
        {
            _elements.Clear();
            _listCache = null;
        }

        /// <summary>
        /// Checks if passed element exists by its ID.
        /// </summary>
        /// <param name="item">Board Element to check.</param>
        /// <returns>If Board Element was found.</returns>
        public bool Contains(BoardElement item)
        {
            Type type = item.GetType();

            if (!_elements.ContainsKey(type))
                return false;

            if (!_elements[type].ContainsKey(item.Id))
                return false;

            return true;
        }

        /// <summary>
        /// Currently unimplemented.
        /// </summary>
        public void CopyTo(BoardElement[] array, int arrayIndex) { }

        /// <summary>
        /// Gets and returns an enumerator of all Board Elements currently stored.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BoardElement> GetEnumerator() => ToList().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Remove a stored Board Element by its ID.
        /// </summary>
        /// <param name="id"></param>
        public bool Remove(Guid id)
        {
            BoardElement? element = Get(id);

            return element != null && Remove(element);
        }

        /// <summary>
        /// Remove stored Board Element.
        /// </summary>
        /// <param name="item">Element to remove.</param>
        /// <returns>If was successfully found and removed.</returns>
        public bool Remove(BoardElement item)
        {
            Type key = item.GetType();
            Guid id = item.Id;

            if (!_elements.ContainsKey(key) || !_elements[key].ContainsKey(id))
                return false;

            _elements[key].Remove(id);
            _listCache = null;

            return true;
        }


        bool ICollection<BoardElement>.Remove(BoardElement item) => Remove(item);

        /// <summary>
        /// Gets all Board Elements and returns them as a list.
        /// </summary>
        /// <returns>List of Board Elements.</returns>
        public List<BoardElement> ToList()
        {
            if (_listCache != null)
                return _listCache;

            List<BoardElement> elements = new List<BoardElement>();
            Type[] keys = [.. _elements.Keys];

            foreach (var key in keys)
            {
                foreach (var child in _elements[key].Values)
                {
                    elements.Add(child);
                }
            }

            _listCache = elements;
            return elements;
        }
    }
}

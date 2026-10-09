using Scrapbook.Core.Properties.Values;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Scrapbook.Core.Properties
{
    public sealed class PropertyDictionary : IDictionary<string, IProperty>
    {
        private readonly Dictionary<string, IProperty> _properties;

        public PropertyDictionary()
        {
            _properties = new Dictionary<string, IProperty>();
        }

        public PropertyDictionary(Dictionary<string, IProperty> properties)
        {
            _properties = properties;
        }

        public void Add(string key, IProperty value) => _properties.Add(key, value);
        public bool Remove(string key) => _properties.Remove(key);
        public void Clear() => _properties.Clear();
        public bool Remove(KeyValuePair<string, IProperty> item) => Remove(item.Key);

        
        #region Inherited
        public void Add(KeyValuePair<string, IProperty> item) => Add(item.Key, item.Value);
        public bool Contains(KeyValuePair<string, IProperty> item) => ContainsKey(item.Key);
        public bool ContainsKey(string key) => _properties.ContainsKey(key);
        public IEnumerator<KeyValuePair<string, IProperty>> GetEnumerator() => _properties.GetEnumerator();
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out IProperty value) => _properties.TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();
        public ICollection<string> Keys => _properties.Keys;
        public ICollection<IProperty> Values => _properties.Values;
        public int Count => _properties.Count;
        public bool IsReadOnly => false;
        public IProperty this[string key]
        {
            get => ((IDictionary<string, IProperty>)_properties)[key];
            set => ((IDictionary<string, IProperty>)_properties)[key] = value;
        }
        public void CopyTo(KeyValuePair<string, IProperty>[] array, int arrayIndex)
        {

        }
        #endregion Inherited
    }
}

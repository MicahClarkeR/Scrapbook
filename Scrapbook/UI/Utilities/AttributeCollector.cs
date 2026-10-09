using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scrapbook.UI.Utilities
{
    public sealed class AttributeCollector<T> where T : Attribute
    {
        public List<KeyValuePair<object, T>> Collect(object target)
        {
            List<KeyValuePair<object, T>> result = [];
            Type type = target.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                T? attribute = property.GetCustomAttribute<T>();
                object? value = property.GetValue(target);

                if (value == null || attribute == null)
                    continue;

                result.Add(new KeyValuePair<object, T>(value, attribute));
            }

            return result;
        }
    }
}

using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Scrapbook.Core.Properties.Values
{
    public interface IPropertyValue : IProperty
    {
        public void SetValue(object value);
        public object GetValue();
    }

    public abstract class PropertyValue : IPropertyValue
    {
        public event EventHandler<PropertyValueChangeEventArgs>? OnValueChange;

        public PropertyValue(EventHandler<PropertyValueChangeEventArgs>? changeHandler = null)
        {
            if (changeHandler != null)
            {
                OnValueChange += changeHandler;
            }
        }

        public object GetValue()
        {
            return InvokeGetValue();
        }
        public void SetValue(object value)
        {
            var oldValue = value;
            InvokeSetValue(value);
            OnValueChange?.Invoke(this, new PropertyValueChangeEventArgs(oldValue, value));
        }
        protected abstract object InvokeGetValue();
        protected abstract void InvokeSetValue(object value);
    }

    public abstract class PropertyValue<T> : PropertyValue
    {
        [XmlIgnore]
        [JsonIgnore]
        public T Value
        {
            get => (T) GetValue();
            set => SetValue(value);
        }

        public PropertyValue(T value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(changeHandler)
        {
            InvokeSetValue(value);
        }
    }
}

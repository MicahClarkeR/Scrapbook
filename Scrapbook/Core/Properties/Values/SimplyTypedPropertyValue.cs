using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Values
{
    public interface ISimplyTypedPropertyValue
    {

    }

    public abstract class SimplyTypedPropertyValue<T> : PropertyValue<T>, ISimplyTypedPropertyValue
    {
        private T _value;

        public SimplyTypedPropertyValue(T value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {
        }

        protected override void InvokeSetValue(object value)
        {
            if (value is T typed && IsValid(typed))
            {
                _value = typed;
            }
        }

        protected override object InvokeGetValue() => _value;

        protected virtual bool IsValid(T value) => true;
    }
}

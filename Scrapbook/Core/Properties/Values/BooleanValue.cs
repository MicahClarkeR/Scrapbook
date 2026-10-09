using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Values
{
    public sealed class BooleanValue : SimplyTypedPropertyValue<bool>
    {
        public BooleanValue(bool value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {
        }

        public override string ToString() => Value.ToString();
        public static explicit operator bool(BooleanValue x) => x.Value;
        public static explicit operator BooleanValue(bool x) => new BooleanValue(x);
    }
}

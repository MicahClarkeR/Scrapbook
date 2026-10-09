using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Values
{
    public sealed class StringValue : SimplyTypedPropertyValue<string>
    {
        public StringValue(string value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {
        }

        public StringValue() : base(string.Empty)
        {
        }

        public override string ToString() => Value;

        public static explicit operator string(StringValue x) => x.Value;
        public static explicit operator StringValue(string x) => new StringValue(x);
    }
}

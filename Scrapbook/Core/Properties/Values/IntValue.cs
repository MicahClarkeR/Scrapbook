using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Values
{
    public class IntValue : SimplyTypedPropertyValue<int>
    {
        public IntValue(int value, EventHandler<PropertyValueChangeEventArgs>? changeHandler = null) : base(value, changeHandler)
        {
        }
        public override string ToString() => Value.ToString();
        public static explicit operator int(IntValue x) => x.Value;
        public static explicit operator IntValue(int x) => new IntValue(x); 
    }
}

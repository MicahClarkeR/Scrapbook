using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Values
{
    public sealed class GuidValue : SimplyTypedPropertyValue<Guid>
    {
        public GuidValue(Guid value) : base(value)
        {
        }
        public GuidValue() : base(Guid.Empty)
        {
        }

        public override string ToString() => Value.ToString();
        public static explicit operator Guid(GuidValue x) => x.Value;
        public static explicit operator GuidValue(Guid x) => new GuidValue(x);
    }
}

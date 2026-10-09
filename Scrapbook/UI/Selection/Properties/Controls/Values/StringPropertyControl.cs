using Scrapbook.Core.Properties.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public sealed class StringPropertyControl : SimplePropertyControl<StringValue>, IPropertyControl
    {
        public StringPropertyControl(string label, StringValue value) : base(label, value)
        {
        }

        protected override StringValue? ValueTypeFromString(string value) => new StringValue(value);
    }
}

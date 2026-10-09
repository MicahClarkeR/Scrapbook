using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Selection;
using Scrapbook.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public sealed class GuidPropertyControl : SimplePropertyControl<GuidValue>, IPropertyControl
    {
        public GuidPropertyControl(string label, GuidValue value) : base(label, value)
        {
        }

        protected override GuidValue? ValueTypeFromString(string value)
        {
            if (Guid.TryParse(value, out Guid guid))
            {
                return new GuidValue(guid);
            }

            return null;
        }
    }
}

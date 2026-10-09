using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Selection.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public sealed class IntPropertyControl : SimplePropertyControl<IntValue>, IPropertyControl
    {
        private int _validValue = 0;

        public IntPropertyControl(string label, IntValue value) : base(label, value)
        {
            _validValue = value.Value;
        }

        protected override IntValue? ValueTypeFromString(string value)
        {
            if (int.TryParse(value, out var intRaw))
            {
                IntValue intValue = new IntValue(intRaw); return intValue;
            }

            return null;
        }

        protected override void SetupInput(TextBox input)
        {
            base.SetupInput(input);

            input.TextInput += (s, e) =>
            {
                if (int.TryParse(e.Text, out var intRaw))
                    _validValue = intRaw;
                else
                    Value.SetValue(_validValue);
            };
            input.Height = 20;
        }
    }
}

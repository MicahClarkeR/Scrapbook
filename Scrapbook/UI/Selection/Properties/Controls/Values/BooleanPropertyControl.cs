using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Selection.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public sealed class BooleanPropertyControl : PropertyControl, IPropertyControl
    {
        private readonly string _label;
        private readonly BooleanValue _value;

        public BooleanPropertyControl(string label, BooleanValue value) : base()
        {
            _label = label;
            _value = value;
        }

        public override void Build(PropertyGrid grid, int row = 0)
        {
            var checkbox = new CheckBox()
            {
                IsChecked = _value.Value
            };

            checkbox.Checked += (s, e) => _value.SetValue(checkbox.IsChecked);
            checkbox.Unchecked += (s, e) => _value.SetValue(checkbox.IsChecked);

            grid.AddRow(_label, checkbox);
        }
    }
}

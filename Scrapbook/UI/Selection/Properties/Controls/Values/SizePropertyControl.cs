using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Controls;
using Scrapbook.UI.Selection.Properties;
using Scrapbook.UI.Selection.Properties.GridRows;
using Scrapbook.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public sealed class SizePropertyControl : PropertyControl, IPropertyControl
    {
        public readonly string Label;
        public readonly SizeValue Value;

        private TextBoxFocusListener _widthListener, _heightListener;
        private TextBox _widthBox, _heightBox;

        public SizePropertyControl(string label, SizeValue value)
        {
            Label = label;
            Value = value;
        }

        public override void Build(PropertyGrid grid, int row = 0)
        {
            grid.AddSubGrid
            (
                [
                    new LabelledRow(new()
                    {
                        Content = Label,
                        HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                    }),
                    new LabelledElementRow("Width", _widthBox = new PropertyTextBox()
                    {
                        Text = Value.Width.ToString(),
                    }),
                    new LabelledElementRow("Height", _heightBox = new PropertyTextBox()
                    {
                        Text = Value.Height.ToString(),
                    })
                ]
            );

            _widthListener = new TextBoxFocusListener(_widthBox);
            _widthListener.TextBoxFocusValueChange += (s, e) => UpdateWidth();
            _heightListener = new TextBoxFocusListener(_heightBox);
            _heightListener.TextBoxFocusValueChange += (s, e) => UpdateHeight();

        }

        private void UpdateWidth()
        {
            string gotValue = _widthListener.GotFocusValue;
            string lostValue = _widthListener.LostFocusValue;

            if (float.TryParse(lostValue, out float width))
            {
                Value.Width = width;
            }
            else
            {
                _widthBox.Text = gotValue;
            }
        }

        private void UpdateHeight()
        {
            string gotValue = _heightListener.GotFocusValue;
            string lostValue = _heightListener.LostFocusValue;

            if (float.TryParse(lostValue, out float width))
            {
                Value.Height = width;
            }
            else
            {
                _heightBox.Text = gotValue;
            }
        }
    }
}

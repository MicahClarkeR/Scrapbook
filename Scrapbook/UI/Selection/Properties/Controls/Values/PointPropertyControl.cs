using Scrapbook.Core.Properties.Values;
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
    public sealed class PointPropertyControl : PropertyControl, IPropertyControl
    {
        public readonly string Label;
        public readonly PointValue Value;

        private TextBoxFocusListener _xListener, _yListener;
        private TextBox _xBox, _yBox;

        public PointPropertyControl(string label, PointValue value)
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
                    new LabelledElementRow("X", _xBox = new TextBox()
                    {
                        Text = Value.X.ToString(),
                    }),
                    new LabelledElementRow("Y", _yBox = new TextBox()
                    {
                        Text = Value.Y.ToString(),
                    })
                ]
            );

            _xListener = new TextBoxFocusListener(_xBox);
            _xListener.TextBoxFocusValueChange += (s, e) => UpdateX();
            _yListener = new TextBoxFocusListener(_yBox);
            _yListener.TextBoxFocusValueChange += (s, e) => UpdateY();

        }

        private void UpdateX()
        {
            string gotValue = _xListener.GotFocusValue;
            string lostValue = _xListener.LostFocusValue;

            if (float.TryParse(lostValue, out float x))
            {
                Value.X = x;
            }
            else
            {
                _xBox.Text = gotValue;
            }
                
        }

        private void UpdateY()
        {
            string gotValue = _yListener.GotFocusValue;
            string lostValue = _yListener.LostFocusValue;

            if (float.TryParse(lostValue, out float y))
            {
                Value.Y = y;
            }
            else
            {
                _yBox.Text = gotValue;
            }
        }
    }
}

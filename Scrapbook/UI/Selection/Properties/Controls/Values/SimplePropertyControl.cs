using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Selection.Properties;
using Scrapbook.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public abstract class SimplePropertyControl<T> : PropertyControl where T : ISimplyTypedPropertyValue
    {
        public readonly string Label;
        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }
        private T _value;

        private TextBoxFocusListener _inputListener;
        private TextBox _input;
        private string _focusValue = string.Empty;

        public SimplePropertyControl(string label, T value)
        {
            Label = label;
            Value = value;
        }

        public override void Build(PropertyGrid grid, int row = 0)
        {
            Label label = new Label()
            {
                Content = Label,
                HorizontalContentAlignment = HorizontalAlignment.Right
            };
            _input = new TextBox();
            SetupInput(_input);
            _input.Text = Value.ToString();
            _inputListener = new TextBoxFocusListener(_input);
            _inputListener.TextBoxFocusValueChange += InputValueChange;

            grid.AddRow(label, _input);
        }

        protected virtual void SetupInput(TextBox input)
        { }

        private void InputValueChange(object? sender, TextBoxFocusValueChangeEventArgs e)
        {
            if (ValueTypeFromString(e.LostFocusValue) is T value && value != null)
            {
                var oldValue = GetValue();
                bool updated = SetValue(value);

                if (updated)
                    RaisePropertyChanged(new(oldValue, value));
            }
            else
            {
                _input.Text = e.GotFocusValue.ToString();
            }
        }

        protected abstract T? ValueTypeFromString(string value);

        private T GetValue() => _value;

        private bool SetValue(T value)
        {
            if (!value.Equals(_value))
            {
                _value = value;

                return true;
            }

            return false;
        }
    }
}

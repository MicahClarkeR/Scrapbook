using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Utilities
{
    public class TextBoxFocusValueChangeEventArgs : EventArgs
    {
        public readonly string GotFocusValue, LostFocusValue;

        public TextBoxFocusValueChangeEventArgs(string gotFocusValue, string lostFocusValue)
        {
            GotFocusValue = gotFocusValue;
            LostFocusValue = lostFocusValue;
        }
    }

    public interface ITextBoxFocusValueGatherer
    {

    }

    public class TextBoxFocusListener : ITextBoxFocusValueGatherer
    {
        public string GotFocusValue { get; private set; }
        public string LostFocusValue { get; private set; }

        public event EventHandler<TextBoxFocusValueChangeEventArgs> TextBoxFocusValueChange;

        private readonly TextBox _textbox;

        public TextBoxFocusListener(TextBox textbox)
        {
            _textbox = textbox;

            SetupControl();
        }

        private void SetupControl()
        {
            _textbox.GotFocus += GotFocus;
            _textbox.LostFocus += LostFocus;
        }

        private void GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            GotFocusValue = _textbox.Text;
        }

        private void LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            LostFocusValue = _textbox.Text;

            if (GotFocusValue != LostFocusValue)
                TextBoxFocusValueChange?.Invoke(this, new TextBoxFocusValueChangeEventArgs(GotFocusValue, LostFocusValue));
        }
    }
}

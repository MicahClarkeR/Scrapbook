using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Scrapbook.UI.Selection.Properties.Controls.Interaction.Args
{
    public class ButtonClickArgs : InteractArgs
    {
        public readonly RoutedEventArgs MouseButtonArgs;

        public ButtonClickArgs(RoutedEventArgs args)
        {
            MouseButtonArgs = args;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Selection.Properties.Events
{
    public sealed class PropertyRowAddedEventArgs : EventArgs
    {
        public readonly int Number;

        public PropertyRowAddedEventArgs(int number)
        {
            Number = number;
        }
    }
}

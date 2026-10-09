using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Selection.Events
{
    public sealed class PropertyValueChangeEventArgs : EventArgs
    {
        public readonly object OldValue, NewValue;

        public PropertyValueChangeEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}

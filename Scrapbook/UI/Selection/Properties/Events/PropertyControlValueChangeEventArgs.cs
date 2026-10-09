using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Properties.Events
{
    public class PropertyControlValueChangeEventArgs : EventArgs
    {
        public readonly object OldValue, NewValue;

        public PropertyControlValueChangeEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}

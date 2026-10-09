using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Events
{
    public class ValueChangeEventArgs(string property, object oldValue, object newValue) : EventArgs()
    {
        public readonly string Property = property;
        public readonly object OldValue = oldValue, NewValue = newValue;

        public T GetOld<T>() => (T) OldValue;
        public T GetNew<T>() => (T) NewValue;
    }
}

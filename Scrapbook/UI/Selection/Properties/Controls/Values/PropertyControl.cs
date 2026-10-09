using Scrapbook.UI.Properties.Events;
using Scrapbook.UI.Selection.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Properties.Controls.Values
{
    public interface IPropertyControl
    {
        public void Build(PropertyGrid grid, int row = 0);
    }

    public abstract class PropertyControl : Control, IPropertyControl
    {
        public abstract void Build(PropertyGrid grid, int row = 0);
        public event EventHandler<PropertyControlValueChangeEventArgs>? PropertyChanged;

        protected void RaisePropertyChanged(PropertyControlValueChangeEventArgs e) => PropertyChanged?.Invoke(this, e);
    }
}

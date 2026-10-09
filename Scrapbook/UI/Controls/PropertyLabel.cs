using Scrapbook.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Controls
{
    public class PropertyLabel : Label
    {
        public new int Height { get; set; } = GlobalDefaults.UI.GridElementHeight;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            base.Height = Height;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            Padding = new System.Windows.Thickness(0, 0, 5, 0);
        }
    }
}

using Scrapbook.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Controls
{
    public class PropertyTextBox : TextBox
    {
        public new int Height { get; set; } = GlobalDefaults.UI.GridElementHeight;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            base.Height = Height;
        }
    }
}

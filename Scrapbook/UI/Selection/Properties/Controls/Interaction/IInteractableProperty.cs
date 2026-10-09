using Scrapbook.Core.Properties;
using Scrapbook.UI.Properties.Controls.Values;
using Scrapbook.UI.Selection.Properties.Controls.Interaction.Args;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Selection.Properties.Controls.Interaction
{
    public interface IInteractableProperty<T> : IPropertyControl where T : InteractArgs
    {
        protected void HandleInteraction(T args);
    }
}

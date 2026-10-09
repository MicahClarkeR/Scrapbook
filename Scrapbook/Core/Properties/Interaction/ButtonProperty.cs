using Scrapbook.UI.Selection.Properties.Controls.Interaction.Args;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Properties.Interaction
{
    public class ButtonProperty : IProperty
    {
        public EventHandler<ButtonClickArgs>? OnClick;
        public readonly string Label;

        public ButtonProperty(string label, EventHandler<ButtonClickArgs>? onClick = null) : base()
        {
            Label = label;

            if (onClick != null)
                OnClick += onClick;
        }
    }
}

using Scrapbook.Core.Properties.Interaction;
using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Properties.Controls.Values;
using Scrapbook.UI.Selection.Events;
using Scrapbook.UI.Selection.Properties.Controls.Interaction.Args;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Scrapbook.UI.Selection.Properties.Controls.Interaction
{
    internal class ButtonPropertyControl :  IInteractableProperty<ButtonClickArgs>, IPropertyControl
    {
        public event EventHandler<ButtonClickArgs>? OnClick;

        public Button Control { get; private set; }

        public string Label => Property.Label;

        public readonly ButtonProperty Property;

        public ButtonPropertyControl(ButtonProperty property) : base()
        {
            Property = property;

            if (property.OnClick != null)
                OnClick += property.OnClick;
        }

        public void Build(PropertyGrid grid, int row = 0)
        {
            Control = new Button()
            {
                Content = Label
            };
            Control.Click += (s, e) => OnClick?.Invoke(s, new ButtonClickArgs(e));

            grid.AddRow(Control);
        }

        public void HandleInteraction(ButtonClickArgs args)
        {

        }
    }
}

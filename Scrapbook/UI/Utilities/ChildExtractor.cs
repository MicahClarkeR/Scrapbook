using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Scrapbook.UI.Utilities
{
    public class ChildExtractor
    {
        public readonly FrameworkElement Element;

        public ChildExtractor(FrameworkElement element)
        {
            Element = element;
        }

        public UIElement[] GetChildren()
        {
            if (Element is Panel panel)
            {
                return [.. panel.Children.Cast<UIElement>()];
            }
            else if (Element is Decorator decorator)
                return [decorator.Child];

            return [];
        }
    }
}

using Scrapbook.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Scrapbook.UI.Selection.Properties.GridRows
{
    public interface IGridRow
    {
        
    }

    public abstract class GridRow : IGridRow
    {

    }

    public class LabelledRow : GridRow
    {
        public readonly Label Label;

        public LabelledRow(Label label) : base()
        {
            Label = label;
        }
    }

    public class ElementRow : GridRow
    {
        public readonly UIElement Element;

        public ElementRow(UIElement element)
        {
            Element = element;
        }
    }

    public class LabelledElementRow : GridRow
    {
        public readonly UIElement Element;
        public readonly PropertyLabel Label;

        public LabelledElementRow(string label, UIElement element) : base()
        {
            Label = new PropertyLabel()
            {
                Content = label,
            };
            Element = element;
        }
    }
}

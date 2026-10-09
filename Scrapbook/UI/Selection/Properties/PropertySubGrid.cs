using Scrapbook.UI.Selection.Properties.Events;
using Scrapbook.UI.Selection.Properties.GridRows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scrapbook.UI.Selection.Properties
{
    public class PropertySubGrid : IPropertyGrid
    {
        public readonly Border Border;

        public ColumnDefinition LeftColumn => _grid.ColumnDefinitions[_properties.LeftColumn];
        public ColumnDefinition RightColumn => _grid.ColumnDefinitions[_properties.RightColumn];

        /// <summary>
        /// Note: This EventHandler will pass data from the child PropertyGrid of this PropertySubGrid that manages its actual content.
        /// </summary>
        public event EventHandler<PropertyRowAddedEventArgs>? OnRowAdded;

        private readonly Grid _grid;
        private readonly PropertyGrid _properties;
        private readonly GridSplitter _splitter;
        private readonly int _splitterColumn;

        public PropertySubGrid(GridSplitter splitter, int splitterColumn)
        {
            _splitter = splitter;
            _splitterColumn = splitterColumn;
            _grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 10)
            };
            _properties = new ChildPropertyGrid(_grid, splitter, splitterColumn);

            Border = new Border()
            {
                BorderThickness = new System.Windows.Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.LightGray),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Padding = new Thickness(2),
                Child = (UIElement) _properties,
                Margin = new Thickness(5, 0, 0, 5)
            };

            _properties.OnRowAdded += (s, e) => OnRowAdded?.Invoke(s, e);
        }

        public void Initalise()
        {
            _properties.Initalise();
        }

        public void AddRow(UIElement content) => _properties.AddRow(content);

        public void AddRow(string label, UIElement element) => _properties.AddRow(label, element);

        public void AddRow(Label label, UIElement element) => _properties.AddRow(label, element);

        public void AddRow(IGridRow row)
        {
            if (row is LabelledRow labelledRow)
                AddRow(labelledRow.Label);
            else if (row is ElementRow elementRow)
                AddRow(elementRow.Element);
            else if (row is LabelledElementRow labelledElementRow)
                AddRow(labelledElementRow.Label, labelledElementRow.Element);
        }

        public void AddSubGrid(IGridRow[] gridRows)
        {
            PropertySubGrid propertyGrid = new PropertySubGrid(_splitter, _splitterColumn);
            propertyGrid.Initalise();

            foreach (IGridRow row in gridRows)
            {
                propertyGrid.AddRow(row);
            }

            AddRow(propertyGrid.Border);
        }

        public void Clear() => _properties.Clear();

        public static explicit operator UIElement(PropertySubGrid grid) => grid.Border;
        public static explicit operator FrameworkElement(PropertySubGrid grid) => grid.Border;
    }
}

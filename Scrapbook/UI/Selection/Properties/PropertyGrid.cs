using Scrapbook.Core.Configuration;
using Scrapbook.UI.Controls;
using Scrapbook.UI.Selection.Properties.Events;
using Scrapbook.UI.Selection.Properties.GridRows;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Scrapbook.UI.Selection.Properties
{
    public interface IPropertyGrid
    {
        public event EventHandler<PropertyRowAddedEventArgs>? OnRowAdded;
        public void Initalise();
        public void AddRow(UIElement content);
        public void AddRow(string label, UIElement element);
        public void AddRow(Label label, UIElement element);
        public void AddSubGrid(IGridRow[] gridRows);
        public void Clear();
    }

    public abstract class PropertyGrid : IPropertyGrid
    {
        public event EventHandler<PropertyRowAddedEventArgs>? OnRowAdded;

        protected Thickness CellThickness => new Thickness(0.5, 1.5, 0.5, 1.5);
        public Grid Grid { get; private set; }
        public int RowCount => Grid.RowDefinitions.Count;
        public int RowIndex => Math.Max(0, RowCount - 1);
        public int LeftColumn = 0;
        public int RightColumn => Grid.ColumnDefinitions.Count - 1;
        public int ColumnCount => Grid.ColumnDefinitions.Count;

        public PropertyGrid(Grid parent)
        {
            Grid = parent;
            Grid.DataContext = this;
        }
        public void Initalise()
        {
            SetUpGrid();
        }

        protected virtual void SetUpGrid()
        {
            Grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                MaxWidth = InternalConfig.PropertyLabelColumnWidth.Max,
                MinWidth = InternalConfig.PropertyLabelColumnWidth.Min,
                Width = new GridLength(InternalConfig.PropertyLabelColumnWidth.Current)
            });
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
        }

        public void AddRow(UIElement content)
        {
            var newRow = new RowDefinition();
            Grid.RowDefinitions.Add(newRow);

            var container = new Border()
            {
                BorderThickness = CellThickness,
                Child = content
            };
            Grid.Children.Add(container);

            Grid.SetRow(container, RowIndex);

            Grid.SetColumn(container, LeftColumn);
            Grid.SetColumnSpan(container, ColumnCount);

            OnRowAdded?.Invoke(this, new PropertyRowAddedEventArgs(RowIndex));
        }

        public void AddRow(string label, UIElement element)
        {
            Label labelControl = new PropertyLabel()
            {
                Content = label
            };

            AddRow(labelControl, element);
        }

        public void AddRow(Label label, UIElement element)
        {
            Grid.RowDefinitions.Add(new RowDefinition());


            var labelContainer = new Border()
            {
                BorderThickness = CellThickness,
                Child = label
            };
            Grid.Children.Add(labelContainer);

            var elementContainer = new Border()
            {
                BorderThickness = CellThickness,
                Child = element
            };
            Grid.Children.Add(elementContainer);

            Grid.SetRow(labelContainer, RowIndex);
            Grid.SetRow(elementContainer, RowIndex);

            Grid.SetColumn(labelContainer, LeftColumn);
            Grid.SetColumn(elementContainer, RightColumn);

            OnRowAdded?.Invoke(this, new PropertyRowAddedEventArgs(RowIndex));
        }

        public void AddSubGrid(IGridRow[] gridRows)
        {
            var splitter = GetSplitter();
            var column = GetSplitterColumn();
            PropertySubGrid propertyGrid = new PropertySubGrid(splitter, column);
            propertyGrid.Initalise();

            foreach (IGridRow gridRow in gridRows)
            {
                propertyGrid.AddRow(gridRow);
            }

            var gridBorder = propertyGrid.Border;
            Grid.Children.Add(gridBorder);

            var newRow = new RowDefinition();

            Grid.RowDefinitions.Add(newRow);

            Grid.SetRow(gridBorder, RowIndex);
            // Grid.SetRowSpan(gridBorder, gridRows.Length);

            Grid.SetColumn(gridBorder, 0);
            Grid.SetColumnSpan(gridBorder, 3);
        }

        public void Clear()
        {
            Grid.Children.Clear();
            Grid.RowDefinitions.Clear();
        }

        protected double CalculateRowHeight(params FrameworkElement[] children)
        {
            double height = -1;

            foreach (FrameworkElement child in children)
            {
                if (child.Height > height)
                    height = child.Height;
            }

            if (height != -1)
            {
                height += CellThickness.Top + CellThickness.Bottom;
            }

            return height;
        }

        protected double CalculateRowWidth(params FrameworkElement[] children)
        {
            double width = -1;

            foreach (FrameworkElement child in children)
            {
                if (child.Width > width)
                    width = child.Width;
            }

            if (width != -1)
            {
                width += CellThickness.Left + CellThickness.Right;
            }

            return width;
        }

        protected abstract GridSplitter GetSplitter();
        protected abstract int GetSplitterColumn();
        public static implicit operator UIElement(PropertyGrid grid) => grid.Grid;
    }

    public class MasterPropertyGrid : PropertyGrid
    {
        protected int SplitterColumn { get; private set; }

        private GridSplitter _splitter;

        public MasterPropertyGrid(Grid parent, int splitterColumn = 1) : base(parent)
        {
            SplitterColumn = splitterColumn;

            _splitter = new GridSplitter()
            {
                ResizeDirection = GridResizeDirection.Columns,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(70, 200, 200, 200)),
                Focusable = false
            };
        }

        protected override void SetUpGrid()
        {
            base.SetUpGrid();

            Grid.ColumnDefinitions.Insert(SplitterColumn, new ColumnDefinition()
            {
                Width = new GridLength(5),
            });
            _splitter.DragCompleted += SplitterDragCompleted;
            _splitter.DragDelta += SplitterDragDelta;
            Grid.Children.Add(_splitter);

            Grid.SetColumn(_splitter, SplitterColumn);
            Grid.SetRow(_splitter, 0);

            OnRowAdded += GridOnRowAdded;
        }

        private void SplitterDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var width = Math.Round(Grid.ColumnDefinitions[LeftColumn].Width.Value);
            var columnWidth = new GridLength(width);

            foreach (UIElement element in Grid.Children)
            {
                if (element is Border border && border.Child is FrameworkElement framework)
                {
                    if (framework.DataContext is ChildPropertyGrid subGrid)
                    {

                        subGrid.Grid.ColumnDefinitions[LeftColumn].Width = columnWidth;
                    }
                }
            }
        }

        private void SplitterDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            var width = Math.Round(Grid.ColumnDefinitions[LeftColumn].Width.Value);
            InternalConfig.PropertyLabelColumnWidthValue = width;

            var columnWidth = new GridLength(InternalConfig.PropertyLabelColumnWidthValue);
            Grid.ColumnDefinitions[LeftColumn].Width = columnWidth;

        }

        private void GridOnRowAdded(object? sender, PropertyRowAddedEventArgs e)
        {
            Grid.SetRowSpan(_splitter, e.Number + 1);

            Grid.Children.Remove(_splitter);
            Grid.Children.Add(_splitter);
        }

        protected override GridSplitter GetSplitter() => _splitter;
        protected override int GetSplitterColumn() => SplitterColumn;
    }

    public class ChildPropertyGrid : PropertyGrid
    {
        private readonly GridSplitter _splitter;
        private readonly int _splitterColumn;

        public ChildPropertyGrid(Grid parent, GridSplitter splitter, int splitterColumn) : base(parent)
        {
            _splitter = splitter;
            _splitterColumn = splitterColumn;
        }

        protected override void SetUpGrid()
        {
            base.SetUpGrid();

            Grid.ColumnDefinitions.Insert(_splitterColumn, new ColumnDefinition()
            {
                Width = new GridLength(5)
            });
        }

        public void SetColumnWidth(int column, double pixelWidth)
        {
            if (Grid.ColumnDefinitions.Count >= column || pixelWidth < 0)
                return;

            Grid.ColumnDefinitions[column].Width = new GridLength(pixelWidth);
        }

        protected override GridSplitter GetSplitter() => _splitter;
        protected override int GetSplitterColumn() => _splitterColumn;
    }
}

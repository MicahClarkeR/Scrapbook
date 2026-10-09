using Scrapbook.Core.Configuration;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Core.Properties;
using Scrapbook.Core.Properties.Values;
using Scrapbook.Editing;
using Scrapbook.Editing.Services;
using Scrapbook.Helper;
using Scrapbook.UI.Handles;
using Scrapbook.UI.Selection;
using Scrapbook.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml.Linq;

namespace Scrapbook.UI.Board
{
    public sealed class BoardCanvas : Canvas, ISelectable
    {
        private readonly BoardEditorSession Session;

        private MoveHandle? _move = null;
        private List<FrameworkElement> _selectedElements = [];

        public BoardCanvas(BoardEditorSession session) : base()
        {
            Session = session;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Width = Session.Document.Canvas.Width;
            Height = Session.Document.Canvas.Height;
            Background = Session.Document.Canvas.Background.ToBrush();

            AllowDrop = true;

            ContextMenu = ContextMenuBuilder.Create()
                .Add("Add Image", async (sender, args) => { await Session.Editor.ImportImageAsync(); })
                .Add("Paste", async (sender, args) => { await Session.Editor.PasteImageAsync(); })
                .Build();
        }

        public PropertyDictionary? GetProperties(BoardEditorService service)
        {
            var dict = new PropertyDictionary();

            dict.Add("Canvas", new SizeValue(Session.Document.Canvas.ToSize(), (s, e) =>
            {
                var size = (Size2D) e.NewValue;

                Width = size.Width;
                Height = size.Height;
            }));

            return dict;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var source = e.Source as FrameworkElement;

            if (source == this || source == null || source is not FrameworkElement || source.DataContext is not BoardElement)
            {
                return;
            }
            if (_move == null && e.LeftButton == MouseButtonState.Pressed)
            {
                _selectedElements = [source];
                var left = Canvas.GetLeft(source);
                var top = Canvas.GetTop(source);
                var position = new Point(left, top);

                _move = new MoveHandle(this, source);
                _move.Moved += HandleMoved;
                _move.MoveFinished += HandleMoveFinished;
            }
            if (e.MiddleButton == MouseButtonState.Pressed)
            {

            }
        }

        private void HandleMoved(object? sender, Handles.Events.HandleMovedEventArgs e)
        {
            foreach (var element in _selectedElements)
            {
                if (element.DataContext is not VisualElement selected)
                    continue;

                var position = selected.DisplayPosition;
                var movedPosition = MathHelper.Add(position, e.Distance);

                if (movedPosition.X < 0)
                    movedPosition.X = 0;

                if (movedPosition.Y < 0)
                    movedPosition.Y = 0;

                if (movedPosition.X + element.ActualWidth > ActualWidth)
                    movedPosition.X = ActualWidth - element.ActualWidth;

                if (movedPosition.Y + element.ActualHeight > ActualHeight)
                    movedPosition.Y = ActualHeight - element.ActualHeight;

                MoveElement(element, movedPosition);
            }
        }

        private void MoveElement(FrameworkElement element, Point position)
        {
            if (element.DataContext is not VisualElement boardElement)
                return;

            boardElement.VisualPosition = position;
        }

        private void HandleMoveFinished(object? sender, Handles.Events.HandleFinishedMovingEventArgs e)
        {
            if (_move == null)
                return;

            foreach (var element in _selectedElements)
            {
                if (element.DataContext is not BoardElement boardElement)
                    return;

                Session.Editor.Elements.MoveElementAsync(_move.TotalCursorDistance, boardElement.Id).Wait();
            }

            _move.Moved -= HandleMoved;
            _move = null;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (e.Source == this)
                Session.Editor.CurrentSelection.Clear();
        }
    }
}

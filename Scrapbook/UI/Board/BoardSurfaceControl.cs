using Scrapbook.Core.Assets;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Events;
using Scrapbook.Editing;
using Scrapbook.Editing.Events;
using Scrapbook.UI.Board;
using Scrapbook.UI.Board.Operations.Elements;
using Scrapbook.UI.Board.Operations.Elements.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Scrapbook.UI.WPF
{
    // WPF pointer/keyboard interaction and visuals
    public class BoardSurfaceControl : Grid
    {
        public BoardEditorSession Session => BoardEditorSession.Instance;
        private BoardDocument Document => Session.Document;
        private AssetCatalogue Assets => Session.Assets;

        private readonly UpdatePositionBoardOperation _moveOperation = new UpdatePositionBoardOperation();
        private readonly UpdateRotationOperation _rotationOperation = new UpdateRotationOperation();
        private readonly UpdateSizeOperation _resizeOperation = new UpdateSizeOperation();

#nullable enable
        private ScrollViewer _scroll;
        public BoardCanvas BoardCanvas { get; private set; }
#nullable disable

        private Point? _previousMousePosition = null;


        public BoardSurfaceControl()
        {
            Session.Editor.DocumentChanged += OnDocumentChanged;
        }

        #region initalise 
        public override void BeginInit()
        {
            base.BeginInit();

            _scroll = CreateScrollViewer();
            BoardCanvas = new BoardCanvas(Session);
            BoardCanvas.MouseMove += BoardCanvasMouseMove;

            _scroll.Content = BoardCanvas;
            Children.Add(_scroll);
        }

        private void BoardCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Point currentPosition = Mouse.GetPosition(_scroll);

                if (_previousMousePosition == null)
                    _previousMousePosition = currentPosition;
                else if (_previousMousePosition != null)
                {
                    Vector distance = (Vector) (_previousMousePosition - currentPosition) * 1;

                    _scroll.ScrollToHorizontalOffset(_scroll.HorizontalOffset + distance.X);
                    _scroll.ScrollToVerticalOffset(_scroll.VerticalOffset + distance.Y);

                    _previousMousePosition = currentPosition;
                }
            }
            else if (_previousMousePosition != null && e.MiddleButton == MouseButtonState.Released)
            {
                _previousMousePosition = null;
            }
        }

        private static ScrollViewer CreateScrollViewer()
        {
            ScrollViewer scroll = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Top,
                Background = new SolidColorBrush(Colors.LightGray),
            };

            return scroll;
        }
        #endregion initalise

        private void OnDocumentChanged(object sender, Editing.Events.DocumentChangeEventArgs e)
        {
            switch (e.Kind)
            {
                case Editing.Events.DocumentChangeEvent.ElementAdded:
                    OnDocumentElementAdded(sender, e);
                    break;
                case Editing.Events.DocumentChangeEvent.ElementChanged:
                    OnDocumentElementUpdated(sender, e);
                    break;
                case Editing.Events.DocumentChangeEvent.DocumentReset:
                    OnDocumentReset();
                    break;
                case Editing.Events.DocumentChangeEvent.ElementRemoved:
                    OnDocumentElementRemoved(sender, e);
                    break;
            }
        }

        private void OnDocumentElementRemoved(object sender, DocumentChangeEventArgs e)
        {
            foreach(Guid id in e.ElementIds)
            {
                RemoveElement(id);
            }
        }

        private void OnDocumentReset()
        {

        }

        private void OnDocumentElementAdded(object sender, Editing.Events.DocumentChangeEventArgs e)
        {
            foreach(Guid id in e.ElementIds)
            {
                AddElement(id);
            }
        }

        private void OnDocumentElementUpdated(object sender, DocumentChangeEventArgs e)
        {
            foreach (Guid id in e.ElementIds)
            {
                ProcessElementChange(id);
            }
        }

        protected virtual void AddElement(Guid id)
        {
            BoardElement element = Document.Elements.Get(id);

            if (element is VisualElement visual)
            {
                visual.Initialise();
                visual.AddToCanvas(BoardCanvas, Session);
                visual.OnVisualPositionChange += (s, e) => VisualPositionOnChange(visual, e);

                UpdatePosition(visual);
                UpdateRotation(visual);

                if (visual is SizedElement sized)
                {
                    UpdateSize(sized);
                }
            }
        }

        private void VisualPositionOnChange(VisualElement visualElement, ValueChangeEventArgs e)
        {
            ElementsBoardOperationModel model = new ElementsBoardOperationModel(visualElement);
            _moveOperation.Execute(Session, model);
        }

        protected virtual void RemoveElement(Guid id)
        {
            if (Document.Elements.Get(id) is not VisualElement element)
                return;

            UIElement visual = element.GetElement();
            BoardCanvas.Children.Remove(visual);
        }

        protected virtual void ProcessElementChange(Guid id)
        {
            BoardElement element = Document.Elements.Get(id);

            if (element is VisualElement visual)
            {
                UpdatePosition(visual);
                UpdateRotation(visual);

                if (visual is SizedElement sized)
                {
                    UpdateSize(sized);
                }
            }
        }

        private void UpdateSize(params SizedElement[] sized)
        {
            SizedElementsBoardOperationModel model = new SizedElementsBoardOperationModel(sized);
            _resizeOperation.Execute(Session, model);
        }

        private void UpdatePosition(params VisualElement[] visual)
        {
            ElementsBoardOperationModel model = new ElementsBoardOperationModel(visual);
            _moveOperation.Execute(Session, model);
        }

        private void UpdateRotation(params VisualElement[] visual)
        {
            ElementsBoardOperationModel model = new ElementsBoardOperationModel(visual);
            _rotationOperation.Execute(Session, model);
        }

        /**
         * Its responsibilities should be limited to interaction and presentation:
         *  display visualElement visuals
         *  transform document coordinates into screen coordinates
         *  pan and zoom
         *  pointer capture
         *  hit testing
         *  drag-box drawing
         *  resize handles
         *  shift-click interpretation
         *  selection outlines
         *  inline text editing overlay
         *  converting gestures into editor operations
         *
         * It should not:
         *  save documents
         *  import assets
         *  mutate asset files directly
         *  build property inspectors
         *  own undo history
         *  compile shaders
         *  serialize the project
         **/
    }
}

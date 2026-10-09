using Scrapbook.Helper;
using Scrapbook.UI.Board;
using Scrapbook.UI.Handles.Events;
using Scrapbook.UI.Utilities;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Scrapbook.UI.Handles
{
    public class MoveHandle : UIHandle
    {
        public event EventHandler<HandleMovedEventArgs>? Moved;
        public event EventHandler<HandleFinishedMovingEventArgs>? MoveFinished;

        public UIElement[] Children => _targetChildExtractor.GetChildren();
        public Vector2 Distance => new Vector2((float)(CurrentPosition.X - (_previousPosition?.X ?? CurrentPosition.X)), (float)(CurrentPosition.Y - (_previousPosition?.Y ?? CurrentPosition.Y)));
        public Vector2 CursorDistance => new Vector2((float)(_cursorPosition.X - (_previousCursorPosition?.X ?? _cursorPosition.X)), (float)(_cursorPosition.Y - (_previousCursorPosition?.Y ?? _cursorPosition.Y)));
        public Vector2 TotalDistance => new Vector2((float)(CurrentPosition.X - StartPosition.X), (float)(CurrentPosition.Y - StartPosition.Y));
        public Vector2 TotalCursorDistance => new Vector2((float)(_cursorPosition.X - _startCursorPosition.X), (float)(_cursorPosition.Y - _startCursorPosition.Y));
        public Point CurrentPosition { get; private set; } = new Point();

        public readonly Point StartPosition;
        private readonly Point _startCursorPosition;

        private ChildExtractor _targetChildExtractor;
        private Rectangle _rectangle;
        private Point _position, _cursorPosition, _relativeCursorStartPositon, _relativeCursorEndPositon;
        private Point? _previousPosition = null, _previousCursorPosition = null;

        public MoveHandle(Panel parent, FrameworkElement target) : base(parent)
        {
            _targetChildExtractor = new ChildExtractor(target);
            _rectangle = new Rectangle()
            {
                Width = target.ActualWidth,
                Height = target.ActualHeight,
                Fill = new SolidColorBrush(Color.FromArgb(100, 150, 150, 220))
            };

            Parent.Children.Add(_rectangle);

            var left = Canvas.GetLeft(target);
            var top = Canvas.GetTop(target);
            CurrentPosition = StartPosition = new Point((double.IsNaN(left)) ? 0 : left, (double.IsNaN(top)) ? 0 : top);
            _cursorPosition = _startCursorPosition = Mouse.GetPosition(Parent);
            _relativeCursorStartPositon = Mouse.GetPosition(_rectangle);

            Canvas.SetLeft(_rectangle, StartPosition.X);
            Canvas.SetTop(_rectangle, StartPosition.Y);
            Canvas.SetZIndex(_rectangle, 100000);

            _rectangle.MouseMove += SelectionMouseMove;
            _rectangle.MouseUp += RectangleMouseUp;
        }

        private void RectangleMouseUp(object sender, MouseButtonEventArgs e)
        {
            Release();
        }

        public override void Release()
        {
            _relativeCursorEndPositon = Mouse.GetPosition(_rectangle);
            Vector2 totalDistance = TotalDistance;

            totalDistance.X -= (float) (_relativeCursorEndPositon.X - _relativeCursorStartPositon.X);
            totalDistance.Y -= (float) (_relativeCursorEndPositon.Y - _relativeCursorStartPositon.Y);

            MoveFinished?.Invoke(this, new HandleFinishedMovingEventArgs(TotalDistance));

            _rectangle.MouseMove -= SelectionMouseMove;
            Parent.Children.Remove(_rectangle);
        }

        private void SelectionMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {

                _cursorPosition = Mouse.GetPosition(Parent);

                if (_previousPosition == null)
                {
                    _previousCursorPosition = _cursorPosition;
                    _previousPosition = CurrentPosition;

                    return;
                }

                var x = Canvas.GetLeft(_rectangle);
                var y = Canvas.GetTop(_rectangle);
                CurrentPosition = new Point(x, y);

                if (CursorDistance.LengthSquared() == 0)
                    return;

                var left = CurrentPosition.X + CursorDistance.X;
                var top = CurrentPosition.Y + CursorDistance.Y;

                left = MathHelper.MinMax(left, 0, (Parent.ActualWidth - _rectangle.ActualWidth));
                top = MathHelper.MinMax(top, 0, (Parent.ActualHeight - _rectangle.ActualHeight));

                if (left <= 0)
                    left = 0;

                Canvas.SetLeft(_rectangle, left);
                Canvas.SetTop(_rectangle, top);

                Moved?.Invoke(this, new HandleMovedEventArgs(CursorDistance));
                _previousCursorPosition = _cursorPosition;
            }
        }
    }
}

using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Events;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing;
using Scrapbook.UI.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Elements
{
    /// <summary>
    /// Base for all Board Elements that will be displayed on the Board Canvas.
    /// </summary>
    public abstract class VisualElement : BoardElement
    {
        /// <summary>
        /// Handler that triggers whenever the visual posiiton of this Visual Element changes.
        /// </summary>
        public event EventHandler<ValueChangeEventArgs>? OnVisualPositionChange;

        /// <summary>
        /// Underlying Position of this Element, note this is not the same as position displayed on Board Canvas. <br />
        /// See <seealso cref="VisualPosition"/> and <seealso cref="DisplayPosition"/>
        /// </summary>
        public Point2D Position
        {
            get => _position;
            set
            {
                _position.Set(value);
                VisualPosition = value;
            }
        }

        /// <summary>
        /// Used to override the position that is displayed on the Board Canvas. <br />
        /// See <seealso cref="Position"/> and <seealso cref="DisplayPosition"/>
        /// </summary>
        public Point? VisualPosition
        {
            get => (_usingVisualPosition) ? (Point) _visualPosition : null;
            set
            {
                _usingVisualPosition = value != null;

                if (value != null)
                    _visualPosition.Set(value);
            }
        }
        private Point2D _visualPosition = new Point2D(0, 0);
        private bool _usingVisualPosition = false;
        
        /// <summary>
        /// The current position that should be displayed on the Board Canvas, if <see cref="VisualPosition"/> is `null` then will use <see cref="Position"/>.
        /// </summary>
        public Point DisplayPosition => VisualPosition ?? Position;
        private Point2D _position { get => VisualData.Position; set => VisualData.Position = value; }

        /// <summary>
        /// Rotation of this Visual Element on the Board Canvas.
        /// </summary>
        public int Rotation { get => VisualData.Rotation; set => VisualData.Rotation = value; }

        /// <summary>
        /// Z index of this Visual Element used for layering on the Board Canvas.
        /// </summary>
        public int ZIndex { get => VisualData.ZIndex; set => VisualData.ZIndex = value; }
        
        /// <summary>
        /// Sets if this Visual Element should be displayed on the Board Canvas.
        /// </summary>
        public bool IsVisisble { get => VisualData.IsVisisble; set => VisualData.IsVisisble = value; }

        private VisualElementData VisualData => (VisualElementData) GetData();

        /// <inheritdoc />
        public override void Initialise()
        {
            base.Initialise();

            _visualPosition.OnChange += VisualPositionOnChange;
        }

        private void VisualPositionOnChange(object? sender, ValueChangeEventArgs e)
        {
            OnVisualPositionChange?.Invoke(this, e);
        }

        /// <summary>
        /// Add this Element to the Canvas.
        /// </summary>
        /// <param name="canvas">Canvas to add this Element to.</param>
        /// <param name="session">Current Editor Session that contains this Element's data.</param>
        /// <returns>If adding to BoardCanvas was successful.</returns>
        public abstract bool AddToCanvas(BoardCanvas canvas, BoardEditorSession session);

        /// <summary>
        /// Get the underlying FrameworkElement that represents this Visual Element.
        /// </summary>
        /// <returns>FrameworkElement that represents this Element.</returns>
        public abstract FrameworkElement? GetElement();
    }
}

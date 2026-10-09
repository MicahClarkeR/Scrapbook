using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using Scrapbook.Helper;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Editing.Operations.Element
{
    public class MoveElementOperation : BoardOperation, ISingleTargetBoardOperation
    {
        public override string Description => "Set the Transform of an Element with the given Id.";
        public Guid Id => _id;

        public readonly Vector2 Distance;

        private readonly Guid _id;

        private Point? _oldValue;

        public MoveElementOperation(Vector2 distance, Guid elementId) : base(DocumentChangeEvent.ElementChanged)
        {
            Distance = distance;
            _id = elementId;
        }

        public override async Task Execute(BoardDocument document)
        {
            VisualElement? element = document.Elements.Get<VisualElement>(Id);

            if (element == null || Distance.Length() == 0)
            {
                AddToQueue = false;
                return;
            }

            _oldValue = element.Position;

            Point newValue = MathHelper.Add(element.Position, Distance);

            newValue.X = Math.Max(0, newValue.X);
            newValue.Y = Math.Max(0, newValue.Y);

            element.Position = newValue;
        }

        public override async Task Undo(BoardDocument document)
        {
            VisualElement? element = document.Elements.Get<VisualElement>(Id);

            if (element == null || _oldValue == null)
                return;

            element.Position = (Point) _oldValue;
        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, Id);
    }
}

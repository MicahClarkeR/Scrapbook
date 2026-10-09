using Scrapbook.Core.Documents;
using Scrapbook.Core.Geometry.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.Core.Geometry.Utilities
{
    public class DocumentBoardHitTester : IBoardHitTester
    {
        public Guid? HitTestPoint(BoardDocument document, Vector2 point)
        {
            Guid? found = null;

            foreach(var element in document.Elements)
            {
                Rect2D? rect = document.GetBounds(element.Id);

                if (rect == null)
                    continue;

                if (rect.Contains(point))
                    found = element.Id;

                if (found != null)
                    break;
            }

            return found;
        }

        public IReadOnlyList<Guid> HitTestRectangle(BoardDocument document, Rect2D rectangle, RectangleSelectionMode mode)
        {
            List<Guid> result = new List<Guid>();

            foreach (var element in document.Elements)
            {
                Rect2D? rect = document.GetBounds(element.Id);

                if (rect == null)
                    continue;

                if (rectangle.Contains(rect) || rect.Contains(rectangle))
                    result.Add(element.Id);
            }

            return result.AsReadOnly();
        }

        
    }
}

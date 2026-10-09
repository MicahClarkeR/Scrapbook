using Scrapbook.Core.Documents;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Scrapbook.Core.Geometry.Interfaces
{
    public interface IBoardHitTester
    {
        Guid? HitTestPoint(BoardDocument document, Vector2 point);
        IReadOnlyList<Guid> HitTestRectangle(BoardDocument document, Rect2D rectangle, RectangleSelectionMode mode);
    }
}

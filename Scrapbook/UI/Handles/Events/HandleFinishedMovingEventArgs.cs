using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.UI.Handles.Events
{
    public class HandleFinishedMovingEventArgs : EventArgs
    {
        public readonly Vector2 TotalDistance;

        public HandleFinishedMovingEventArgs(Vector2 totalDistance)
        {
            TotalDistance = totalDistance;
        }
    }
}

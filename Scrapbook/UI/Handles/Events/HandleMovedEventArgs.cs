using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;

namespace Scrapbook.UI.Handles.Events
{
    public class HandleMovedEventArgs : EventArgs
    {
        public readonly Vector2 Distance;

        public HandleMovedEventArgs(Vector2 distance)
        {
            Distance = distance;
        }
    }
}

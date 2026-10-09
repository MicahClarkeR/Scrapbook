using System.Windows.Controls;

namespace Scrapbook.UI.Handles
{
    public interface IUIHandle
    {
        public void Release();
    }

    public abstract class UIHandle : IUIHandle
    {
        public readonly Panel Parent;

        public UIHandle(Panel parent)
        {
            Parent = parent;
        }
        public abstract void Release();
    }
}

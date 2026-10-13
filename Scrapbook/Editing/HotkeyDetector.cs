using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Scrapbook.Editing
{
    public static class HotkeyDetector
    {
        public static bool Enabled { get; set; } = true;

        private static List<HotkeyInfo> _registeredHotkeys = new List<HotkeyInfo>();

        static HotkeyDetector()
        {
            KeyEventHandler keyHandler = new KeyEventHandler(KeyEventCallback);
            Keyboard.AddKeyUpHandler(MainWindow.Instance, keyHandler);
        }

        public static Guid Register(Key key, ModifierKeys modifiers, EventHandler<KeyEventArgs> handler)
        {
            Guid hotkeyId = Guid.NewGuid();
            HotkeyInfo into = new HotkeyInfo()
            {
                Id = hotkeyId,
                Key = key,
                Modifiers = modifiers,
                Handler = handler
            };

            _registeredHotkeys.Add(into);

            return hotkeyId;
        }

        public static Guid Register(Key key, EventHandler<KeyEventArgs> handler) => Register(key, ModifierKeys.None, handler);

        public static bool Deregister(Guid id)
        {
            HotkeyInfo? toRemove = null;

            for (int i = 0; i < _registeredHotkeys.Count; i++)
            {
                HotkeyInfo info = _registeredHotkeys[i];

                if (info.Id == id)
                {
                    toRemove = info;
                    break;
                }
            }

            if (toRemove != null)
                _registeredHotkeys.Remove((HotkeyInfo) toRemove);

            return toRemove != null;
        }

        private static void KeyEventCallback(object sender, KeyEventArgs e)
        {
            if (!Enabled)
                return;

            foreach(HotkeyInfo info in _registeredHotkeys)
            {
                Key key = e.Key;
                ModifierKeys modifiers = Keyboard.Modifiers;

                if (info.Key == key && info.Modifiers == modifiers)
                {
                    info.Handler.Invoke(sender, e);
                }
            }
        }

        private struct HotkeyInfo
        {
            public Guid Id;
            public Key Key;
            public ModifierKeys Modifiers;
            public EventHandler<KeyEventArgs> Handler;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using static Scrapbook.UI.Utilities.ContextMenuBuilder.SubMenuBuilderParentless;

namespace Scrapbook.UI.Utilities
{
    public class ContextMenuBuilder : IContextMenuBuilder
    {
        private ContextMenu _menu = new ContextMenu();

        private ContextMenuBuilder()
        {

        }

        public virtual IContextMenuBuilder Add(string header, Action<object, RoutedEventArgs> callback)
        {
            MenuItem item = CreateMenuItem(header, callback);
            _menu.Items.Add(item);

            return this;
        }

        public virtual IContextMenuBuilder Add(ContextMenu menu)
        {
            _menu.Items.Add(menu);
            return this;
        }

        public virtual IContextMenuBuilder Add(MenuItem item)
        {
            _menu.Items.Add(item);
            return this;
        }

        public virtual IContextMenuBuilder Add(IContextSubMenuBuilder builder)
        {
            ((SubMenuBuilder)builder).BuildAndReturn(this);
            return this;
        }

        public ContextMenu Build()
        {
            return _menu;
        }

        public IContextSubMenuBuilder CreateSubMenu(string header)
        {
            MenuItem item = new MenuItem()
            {
                Header = header
            };

            return new SubMenuBuilder(this, item);
        }

        public IContextMenuBuilder AddSeparator()
        {
            _menu.Items.Add(new Separator());

            return this;
        }

        private static MenuItem CreateMenuItem(string header, Action<object, RoutedEventArgs> callback)
        {
            MenuItem item = new MenuItem()
            {
                Header = header
            };

            item.Click += callback.Invoke;

            return item;
        }

        private static readonly Regex UrlRegex = new Regex(@"(http|https)://[^\s/$.?#].[^\s]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static IContextMenuBuilder Create()
            => new ContextMenuBuilder();

        public class SubMenuBuilder : ContextMenuBuilder, IContextSubMenuBuilder
        {
            private readonly ContextMenuBuilder _parent;
            private readonly MenuItem _item;

            public SubMenuBuilder(ContextMenuBuilder parent, MenuItem item)
            {
                _parent = parent;
                _item = item;
            }

            public override IContextMenuBuilder Add(string header, Action<object, RoutedEventArgs> callback)
            {
                MenuItem item = CreateMenuItem(header, callback);
                return Add(item);
            }

            public override IContextMenuBuilder Add(MenuItem item)
            {
                _item.Items.Add(item);
                return this;
            }

            public override IContextMenuBuilder Add(ContextMenu menu)
            {
                _item.Items.Add(menu);
                return this;
            }

            public IContextMenuBuilder BuildAndReturn()
            {
                _parent.Add(_item);
                return _parent;
            }

            public IContextMenuBuilder BuildAndReturn(IContextMenuBuilder parent)
            {
                parent.Add(_item);
                return _parent;
            }

            IContextSubMenuBuilder IContextSubMenuBuilder.Add(MenuItem item)
            {
                _item.Items.Add(item);

                return this;
            }

            IContextSubMenuBuilder IContextSubMenuBuilder.Add(ContextMenu menu) => (IContextSubMenuBuilder)Add(menu);

            IContextSubMenuBuilder IContextSubMenuBuilder.Add(string header, Action<object, RoutedEventArgs> callback)
                => (IContextSubMenuBuilder)Add(header, callback);
        }

        public class SubMenuBuilderParentless
        {
            private readonly MenuItem _item;

            public SubMenuBuilderParentless(MenuItem item)
            {
                _item = item;
            }

            public SubMenuBuilderParentless Add(string header, Action<object, RoutedEventArgs> callback)
            {
                MenuItem item = CreateMenuItem(header, callback);
                return Add(item);
            }

            public SubMenuBuilderParentless Add(MenuItem item)
            {
                _item.Items.Add(item);
                return this;
            }

            public MenuItem Build() => _item;

            public SubMenuBuilderParentless CreateSubMenu(string header)
            {
                MenuItem item = new MenuItem()
                { Header = header };
                return new SubMenuBuilderParentless(item);
            }

            public interface IContextMenuBuilder
            {
                IContextMenuBuilder Add(MenuItem item);
                IContextMenuBuilder Add(ContextMenu menu);
                IContextMenuBuilder Add(string header, Action<object, RoutedEventArgs> callback);
                IContextMenuBuilder Add(IContextSubMenuBuilder menu);
                IContextSubMenuBuilder CreateSubMenu(string header);
                IContextMenuBuilder AddSeparator();
                ContextMenu Build();
            }

            public interface IContextSubMenuBuilder : IContextMenuBuilder
            {
                new IContextSubMenuBuilder Add(MenuItem item);
                new IContextSubMenuBuilder Add(ContextMenu menu);
                new IContextSubMenuBuilder Add(string header, Action<object, RoutedEventArgs> callback);
                IContextMenuBuilder BuildAndReturn();
                IContextMenuBuilder BuildAndReturn(IContextMenuBuilder parent);
            }
        }
    }
}

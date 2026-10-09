using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Scrapbook.Helper
{
    internal static class TransformHelper
    {
        public static T? GetRenderTransform<T>(FrameworkElement control) where T : Transform
        {
            control.RenderTransform ??= new TransformGroup();
            return GetTransformImpl<T>(control.RenderTransform);
        }

        public static T? GetLayoutTransform<T>(FrameworkElement control) where T : Transform
        {
            control.LayoutTransform ??= new TransformGroup();
            return GetTransformImpl<T>(control.LayoutTransform);
        }

        private static T? GetTransformImpl<T>(Transform transform) where T : Transform
        {
            if (transform is TransformGroup group)
            {
                foreach (var child in group.Children)
                {
                    if (child is T found)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        public static TransformGroup GetLayoutTransformGroup(FrameworkElement element)
        {
            if (element.LayoutTransform is TransformGroup group)
                return group;

            return (TransformGroup) (element.LayoutTransform = new TransformGroup());
        }
        public static TransformGroup? GetRenderTransformGroup(FrameworkElement element)
        {
            if (element.RenderTransform is TransformGroup group)
                return group;

            if (element.RenderTransform == null)
                return (TransformGroup)(element.RenderTransform = new TransformGroup());

            return null;
        }

        private static void AddTransform<T>(TransformGroup group, T transform) where T : Transform
        {
            for(int i = 0;  i < group.Children.Count; i++)
            {
                if (group.Children[i] is T found)
                {
                    group.Children.Remove(found);
                    break;
                }
            }

            group.Children.Add(transform);
        }

        public static bool AddLayoutTransform<T>(FrameworkElement element, T transform) where T : Transform
        {
            TransformGroup group = GetLayoutTransformGroup(element);

            if (group != null)
                AddTransform<T>(group, transform);

            return group != null;
        }

        public static bool AddRenderTransform<T>(FrameworkElement element, T transform) where T : Transform
        {
            TransformGroup? group = GetRenderTransformGroup(element);
            
            if (group != null)
                AddTransform<T>(group, transform);

            return group != null;
        }
    }
}

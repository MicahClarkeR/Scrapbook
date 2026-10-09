using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Attributes
{
    public enum PropertyGroup
    {
        Common,
        Specific
    }
    public enum PropertyType
    {
        Text
    }

    public sealed class InspectorPropertyAttribute : Attribute
    {
        public readonly PropertyGroup PropertyGroup;
        public readonly PropertyType PropertyType;
        public readonly string Label;

        public InspectorPropertyAttribute(PropertyGroup propertyGroup, PropertyType propertyType, string label)
        {
            PropertyGroup = propertyGroup;
            PropertyType = propertyType;
            Label = label;
        }
    }
}

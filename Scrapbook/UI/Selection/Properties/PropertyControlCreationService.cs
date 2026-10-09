using Scrapbook.Core.Properties;
using Scrapbook.Core.Properties.Interaction;
using Scrapbook.Core.Properties.Values;
using Scrapbook.UI.Properties.Controls;
using Scrapbook.UI.Properties.Controls.Values;
using Scrapbook.UI.Selection.Properties.Controls.Interaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Properties
{
    public static class PropertyControlCreationService
    {
        public static IPropertyControl? GetControl<T>(T value, string label = "Label") where T : IProperty
        {
            var control = PropertyControlLookUp(label, value);

            if (control == null)
                return null;

            // To handle changing values in 'normal' property-value interfaces.
            if (value is PropertyValue propertyValue && control is PropertyControl propertyControl)
            {
                propertyControl.PropertyChanged += (s, e) =>
                {
                    propertyValue.SetValue(e.NewValue);
                };
            }

            return control;
        }

        private static IPropertyControl? PropertyControlLookUp(string label, IProperty value)
        {
            if (value is StringValue stringValue)
                return new StringPropertyControl(label, stringValue);
            else if (value is BooleanValue boolValue)
                return new BooleanPropertyControl(label, boolValue);
            else if (value is GuidValue guidValue)
                return new GuidPropertyControl(label, guidValue);
            else if (value is SizeValue sizeValue)
                return new SizePropertyControl(label, sizeValue);
            else if (value is PointValue pointValue)
                return new PointPropertyControl(label, pointValue);
            else if (value is ButtonProperty buttonProperty)
                return new ButtonPropertyControl(buttonProperty);
            else if (value is IntValue intValue)
                return new IntPropertyControl(label, intValue);

            return null;
        }
    }
}

using Scrapbook.Core.Documents;
using Scrapbook.Core.Properties;
using Scrapbook.Editing.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.UI.Selection
{
    /// <summary>
    /// Used by Board Elements to declare it selectable, giving access to its underlying properties.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Get and return properties.
        /// </summary>
        /// <param name="service">Service used when setting up properties for callback functions.</param>
        /// <returns>Created property dictionary, null if error occured.</returns>
        public PropertyDictionary? GetProperties(BoardEditorService service);
    }
}

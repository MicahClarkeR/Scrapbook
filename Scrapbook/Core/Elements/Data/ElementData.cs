using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements.Data
{
    /// <summary>
    /// Abstract class for all undrlying Element Data that will be used for saving and loading, as well as containing all relevant properties for this. <br />
    /// Should be used to contain anything that should be saved or loaded to file.
    /// </summary>
    public abstract class ElementData : IElementData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}

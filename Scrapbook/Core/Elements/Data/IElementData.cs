using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Core.Elements.Data
{
    /// <summary>
    /// Generic interface for all Element Data used, i.e. used for saving and loading in the applicaiton.
    /// </summary>
    public interface IElementData
    {
        /// <summary>
        /// Unique ID for the Board Element this Element Data represents.
        /// </summary>
        public Guid Id { get; set; }
    }
}

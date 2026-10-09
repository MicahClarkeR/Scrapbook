using Scrapbook.Core.Assets;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing;
using System.Xml.Serialization;

namespace Scrapbook.Core.Documents
{
    /// <summary>
    /// Core Project document to contain metadata about the project as well as its contents and control its formatting.
    /// This document will not reference WPF/UIValues, image/element sources, mouse/keyboard, or controls.
    /// </summary>
    public class BoardDocument
    {
        /// <summary>
        /// Current schema version of this document.
        /// </summary>
        public int SchemaVersion { get; init; } = 1;

        /// <summary>
        /// Unique document ID.
        /// </summary>
        public Guid Id { get; init; } = Guid.NewGuid();
        
        /// <summary>
        /// Name of this docucment that will be used as filename.
        /// </summary>
        public string Name { get; set; } = "Untitled";

        /// <summary>
        /// The underlying canvas for this Board Document;
        /// </summary>
        [XmlElement("Canvas")]
        public CanvasDefinition Canvas { get; set; } = new();

        /// <summary>
        /// Elements stored within this Board Document.
        /// </summary>
        public BoardElements Elements { get; } = [];

        /// <summary>
        /// Asset catalogue for loading and saving assets.
        /// </summary>
        [XmlIgnore]
        public AssetCatalogue Assets => BoardEditorSession.Instance.Assets;

        public BoardDocument()
        {

        }

        /// <summary>
        /// (WIP) Gets the bounds of this element.
        /// </summary>
        /// <param name="id">ID of desired element.</param>
        /// <returns>Rectangle representing the bounds of this element.</returns>
        public Rect2D? GetBounds(Guid id)
        {
            VisualElement? element = Elements.Get<VisualElement>(id);

            if (element == null)
                return null;

            double left = element.Position.X;
            double top = element.Position.Y;

            return new Rect2D((float) left, (float) top, 0, 0);
        }
    }
}

using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Events;
using Scrapbook.Core.Geometry;
using Scrapbook.Core.Properties;
using Scrapbook.Editing.Services;
using Scrapbook.Rendering.Abstractions;
using Scrapbook.UI.Attributes;
using Scrapbook.UI.Selection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Scrapbook.Core.Elements
{
    public interface IBoardElement
    {

    }

    /// <summary>
    /// Base class for all Board Elements, which are responsible for handling data interactions between internal systems. This is not responsible for controlling the displayed user interface.
    /// </summary>
    public abstract class BoardElement : IBoardElement, ISelectable
    {
        public bool IsInitialised { get; private set; } = false;

        /// <summary>
        /// Unique GUID to identify this Board Element.
        /// </summary>
        public Guid Id { get; init; } = Guid.NewGuid();

        public BoardElement() : base()
        {
            StartInitialising();
        }

        private void StartInitialising()
        {
            if (!IsInitialised)
            {
                Initialise();
                IsInitialised = true;
            }
        }

        /// <summary>
        /// Board Element sets itself up, i.e. the data requried for usage.
        /// </summary>
        public virtual void Initialise()
        {
            if (IsInitialised)
                return;
        }

        /// <summary>
        /// Initiate property collection for the Board Element.
        /// </summary>
        /// <param name="service">Service which provides the function used by Board Elements, such as 'SetElementPositionAsync' etc.</param>
        /// <returns>Built Property Dictionary contaiing this Element's data properties.</returns>
        public PropertyDictionary? GetProperties(BoardEditorService service)
        {
            return GetElementProperties(service);
        }

        /// <summary>
        /// Serialise this BoardElement's properties to be saved into XML.
        /// </summary>
        /// <param name="writer"></param>
        public void Serialise(XmlWriter writer)
        {
            Type type = GetType();
            XmlSerializer serialiser = new XmlSerializer(type);

            SerialiseData(writer, serialiser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="serialiser"></param>
        protected virtual void SerialiseData(XmlWriter writer, XmlSerializer serialiser) => serialiser.Serialize(writer, this);

        /// <summary>
        /// Implementation method to build this Board Element's properties.
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        protected abstract PropertyDictionary? GetElementProperties(BoardEditorService service);

        /// <summary>
        /// Get the Element Data for this Board Element.
        /// </summary>
        /// <returns>This Board Element's data.</returns>
        public abstract IElementData GetData();
    }
}
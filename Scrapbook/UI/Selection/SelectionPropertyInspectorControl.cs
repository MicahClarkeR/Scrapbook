using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Properties;
using Scrapbook.Core.Properties.Values;
using Scrapbook.Editing;
using Scrapbook.Editing.Selection;
using Scrapbook.Editing.Services;
using Scrapbook.Helper;
using Scrapbook.UI.Attributes;
using Scrapbook.UI.Properties;
using Scrapbook.UI.Selection.Properties;
using Scrapbook.UI.WPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Scrapbook.UI.Selection
{
    /// <summary>
    /// Responsible for maintaining and managing underlying UI that displays properties of selected board Elements to the user.
    /// </summary>
    public class SelectionPropertyInspectorControl
    {
        /// <summary>
        /// Getter for currently selected Element or returns Board Canvas if there is no selection.
        /// </summary>
        private ISelectable? SelectedElement => Selection.PrimaryId is Guid id ? Document.Elements.Get(id) : _boardSurface.BoardCanvas;

        /// <summary>
        /// Gets SelectionState from the current Session.
        /// </summary>
        private SelectionState Selection => Session.Selection;

        /// <summary>
        /// Gets the Board Document from the current Session.
        /// </summary>
        private BoardDocument Document => Session.Document;

        /// <summary>
        /// Gets the Board Editor service from current Session.
        /// </summary>
        private BoardEditorService Editor => Session.Editor;

        private readonly BoardEditorSession Session;
        private readonly BoardSurfaceControl _boardSurface;

        private readonly Grid _owner;
        private PropertyGrid _grid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="session"></param>
        /// <param name="boardSurface"></param>
        public SelectionPropertyInspectorControl(Grid owner, BoardEditorSession session, BoardSurfaceControl boardSurface)
        {
            _owner = owner;
            _grid = new MasterPropertyGrid(_owner);
            Session = session;
            _boardSurface = boardSurface;

            _grid.Initalise();

            _owner.Loaded += OwnerLoaded;
            _owner.Unloaded += OwnerUnloaded;
        }
        
        
        private void OwnerLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Setup callbacks for the Editor and Selection.
            Editor.DocumentChanged -= DocumentChanged;
            Editor.DocumentChanged += DocumentChanged;
            Selection.Changed -= SelectionChanged;
            Selection.Changed += SelectionChanged;
        }

        private void DocumentChanged(object? sender, Editing.Events.DocumentChangeEventArgs e)
        {
            // When the loaded document is changed, refresh the the shown properties.
            RefreshSelection();
        }

        private void OwnerUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Selection.Changed -= SelectionChanged;
        }

        private void SelectionChanged(object? sender, Editing.Selection.Events.SelectionChangedEventArgs e)
        {
            // When the selected element(s) are changed, refresh the the shown properties.
            RefreshSelection();
        }

        internal void RefreshSelection()
        {
            RebuildInspector();
        }

        /// <summary>
        /// Clears shown UI and either rebuild shown properties or show nothing if there is no selection.
        /// </summary>
        private void RebuildInspector()
        {
            _owner.RowDefinitions.Clear();
            _owner.Children.Clear();

            if (SelectedElement is null)
            {
                // Show nothing in Property Inspector as there is no selection.
                return;
            }

            BuildElementProperties(SelectedElement);
        }

        /// <summary>
        /// Rebuilds and shows the underlying properties for the passed Element.
        /// </summary>
        /// <param name="element"></param>
        private void BuildElementProperties(ISelectable element)
        {
            PropertyDictionary properties = element.GetProperties(Editor) ?? [];

            int row = 0;
            foreach(var (label, property) in properties)
            {
                if (property is not IProperty value)
                    continue;

                var propertyControl = PropertyControlCreationService.GetControl(value, label);

                propertyControl?.Build(_grid, row++);
            }
        }
    }
}

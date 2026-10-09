using Scrapbook.Core.Assets;
using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.History;
using Scrapbook.Editing.Selection;
using Scrapbook.Editing.Services;

namespace Scrapbook.Editing
{
    public sealed class BoardEditorSession
    {
        public static BoardEditorSession Instance { get; private set; }

        public event EventHandler<EditorDocumentSwappedEventArgs>? DocumentSwapped;

        public BoardDocument Document
        {
            get => _document;
            set
            {
                _document = value;
                DocumentSwapped?.Invoke(this, new(value));
            }
        }
        private BoardDocument _document;
        public SelectionState Selection { get; }
        public OperationHistory History { get; }
        public AssetCatalogue Assets { get; }
        public BoardEditorService Editor { get; }

        public BoardEditorSession(BoardDocument? document = null)
        {
            Document = document ?? new BoardDocument();
            Selection = new SelectionState();
            History = new OperationHistory();
            Assets = new AssetCatalogue();
            Editor = new BoardEditorService();

            Instance = this;
        }

    }
}
using Scrapbook.Core.Assets;
using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.History;
using Scrapbook.Editing.Operations.Interfaces;
using Scrapbook.Editing.Selection;

namespace Scrapbook.Editing.Services
{
    public abstract class BoardService
    {
        private static event EventHandler<DocumentChangeEventArgs>? GlobalDocumentChanged;

        public SelectionState CurrentSelection => Selection;

        protected BoardDocument Document => _session.Document;
        protected SelectionState Selection => _session.Selection;
        protected OperationHistory History => _session.History;
        protected AssetCatalogue Assets => _session.Assets;

        private BoardEditorSession _session => BoardEditorSession.Instance;

        public event EventHandler<DocumentChangeEventArgs>? DocumentChanged
        {
            add => GlobalDocumentChanged += value;
            remove => GlobalDocumentChanged -= value;
        }

        public BoardService()
        {
        }

        protected async Task ExecuteOperationAsync(BoardOperation operation, DocumentChangeEventArgs eventArgs)
        {
            await History.Execute(operation, Document);
            NotifyDocumentChanged(eventArgs);
        }
        protected async Task ExecuteOperationAsync(BoardOperation operation) => await ExecuteOperationAsync(operation, operation.EventArgs);

        public void NotifyDocumentChanged(DocumentChangeEventArgs change)
        {
            GlobalDocumentChanged?.Invoke(this, change);
        }
    }
}

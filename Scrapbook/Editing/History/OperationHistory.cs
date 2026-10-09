using Scrapbook.Core.Documents;
using Scrapbook.Editing.Operations.Interfaces;
using System.Windows;

namespace Scrapbook.Editing.History
{
    public sealed class OperationHistory
    {
        private readonly Stack<BoardOperation> _undo = new Stack<BoardOperation>(50);
        private readonly Stack<BoardOperation> _redo = new Stack<BoardOperation>(50);

        public async Task Execute(BoardOperation operation, BoardDocument document)
        {
            try
            {
                await operation.Execute(document);

                if (operation.AddToQueue)
                {
                    if (_undo.Count == _undo.Capacity)
                    {
                        BoardOperation last = _undo.Last();
                        last.CleanUp(document);
                    }

                    _undo.Push(operation);
                }
            }
            catch (Exception ex)
            {
                // Todo: Make more robust later.
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task Undo(BoardDocument document)
        {
            if (!_undo.TryPop(out var operation))
                return;

            await operation.Undo(document);
            _redo.Push(operation);
        }

        public async Task Redo(BoardDocument document)
        {
            if (!_redo.TryPop(out var operation))
                return;

            await operation.Redo(document);
            _undo.Push(operation);

            BoardEditorSession.Instance.Editor.NotifyDocumentChanged(operation.EventArgs);
        }
    }
}
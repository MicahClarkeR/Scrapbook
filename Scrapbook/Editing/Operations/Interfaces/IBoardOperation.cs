using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Operations.Interfaces
{
    public interface IBoardOperation
    {
        Task Execute(BoardDocument document);
        Task Undo(BoardDocument document);
        Task Redo(BoardDocument document);
        void CleanUp(BoardDocument document);
    }

    public abstract class BoardOperation : IBoardOperation
    {
        public bool AddToQueue { get; protected set; } = true;
        public abstract string Description { get; }
        public readonly DocumentChangeEvent EventType;
        public DocumentChangeEventArgs EventArgs => GetChangeEventArgs();

        public BoardOperation(DocumentChangeEvent eventType)
        {
            EventType = eventType;
        }

        public abstract Task Execute(BoardDocument document);
        public abstract Task Undo(BoardDocument document);
        public virtual async Task Redo(BoardDocument document) => await Execute(document);
        public virtual void CleanUp(BoardDocument document)
        {

        }
        protected abstract DocumentChangeEventArgs GetChangeEventArgs();
    }

    public interface ISingleTargetBoardOperation : IBoardOperation
    {
        Guid Id { get; }
    }

    public interface IMultipleTargetBoardOperation : IBoardOperation
    {
        List<Guid> Ids { get; protected set; }
    }
}

using Scrapbook.Core.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Events
{
    public class EditorDocumentSwappedEventArgs : EventArgs
    {
        public readonly BoardDocument Document;

        public EditorDocumentSwappedEventArgs(BoardDocument document)
        {
            Document = document;
        }
    }
}

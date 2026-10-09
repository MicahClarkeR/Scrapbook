using Scrapbook.Core.Elements;
using Scrapbook.Editing;
using Scrapbook.Helper;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Input;

namespace Scrapbook.UI.Board.Operations
{
    public interface IBoardOperation
    {
        bool Execute(BoardEditorSession session, IBoardOperationModel model);
    }

    public abstract class BoardOperation<Q> : IBoardOperation where Q : IBoardOperationModel
    {
        public abstract bool Execute(BoardEditorSession session, Q model);

        public bool Execute(BoardEditorSession session, IBoardOperationModel model)
        {
            if (!TypeHelper.ConfirmType(model, typeof(Q)))
                return false;

            return Execute(session, (Q) model);
        }
    }
}

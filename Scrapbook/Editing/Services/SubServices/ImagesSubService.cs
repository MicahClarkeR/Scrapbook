using Scrapbook.Core.Assets;
using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.History;
using Scrapbook.Editing.Operations.Element;
using Scrapbook.Editing.Selection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Editing.Services.SubServices
{
    public sealed class ImagesSubService : BoardService, ISubService
    {
        public ImagesSubService() : base()
        {
        }

        public async Task SetMaintainAspectRatioAsync(bool value, Guid elementId)
        {
            var operation = new SetImageAspectRatioOperation(value, elementId);
            var args = new DocumentChangeEventArgs(DocumentChangeEvent.ElementChanged, elementId);
            await ExecuteOperationAsync(operation, args);
        }
    }
}

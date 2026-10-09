using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Assets;
using Scrapbook.Editing.Operations.Element.Images;
using Scrapbook.Editing.Operations.Interfaces;
using System.Windows;

namespace Scrapbook.Editing.Operations.Element
{
    public class ImportImageElementOperation : BoardOperation
    {
        public override string Description => "Import and add an image to the canvas.";
        public Guid? ElementId { get; private set; }

        private readonly ImportImageAssetOperation _importOperation;
        private AddImageElementOperation _addOperation;

        private Guid AssetId = Guid.Empty;

        public ImportImageElementOperation(Point? position = null) : base(DocumentChangeEvent.ElementAdded)
        {
            _importOperation = new ImportImageAssetOperation(position ?? new Point(0, 0));
        }

        public override async Task Execute(BoardDocument document)
        {
            await _importOperation.Execute(document);
            AssetId = _importOperation.AssetId ?? throw new Exception("No Asset selected during image import operation.");
            
            _addOperation = new AddImageElementOperation(AssetId);
            await _addOperation.Execute(document);

            ElementId = _addOperation.ElementId;
        }

        public override async Task Undo(BoardDocument document)
        {
            await _importOperation.Undo(document);
            await _addOperation.Undo(document);
        }

        public override async Task Redo(BoardDocument document)
        {
            await _addOperation.Redo(document);
            ElementId = _addOperation.ElementId;
        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, _importOperation.AssetId ?? Guid.Empty, ElementId ?? Guid.Empty);
    }
}

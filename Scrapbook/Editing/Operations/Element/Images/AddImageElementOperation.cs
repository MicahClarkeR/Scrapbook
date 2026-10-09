using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;

namespace Scrapbook.Editing.Operations.Element.Images
{
    internal class AddImageElementOperation : BoardOperation
    {
        public override string Description => "Adds an image to the Canvas using the Asset ID of an imported image.";

        public readonly Guid AssetId;
        public Guid? ElementId;

        public AddImageElementOperation(Guid assetId) : base(DocumentChangeEvent.ElementAdded)
        {
            AssetId = assetId;
        }

        public override Task Execute(BoardDocument document)
        {
            ImageElement image = CreateElement(document);
            document.Elements.Add(image);

            ElementId = image.Id;

            return Task.CompletedTask;
        }

        public override Task Undo(BoardDocument document)
        {
            if (ElementId != null)
                BoardEditorSession.Instance.Editor.RemoveElement((Guid)ElementId);

            document.Assets.Trash(AssetId);

            return Task.CompletedTask;
        }

        public override Task Redo(BoardDocument document)
        {
            document.Assets.Restore(AssetId);

            ImageElement image = CreateElement(document);
            document.Elements.Add(image);

            return Task.CompletedTask;
        }

        private ImageElement CreateElement(BoardDocument document)
        {
            ImageAsset asset = document.Assets.Get<ImageAsset>(AssetId) ?? throw new Exception($"Asset {AssetId} could not be found.");
            Size2D size = new Size2D(asset.PixelWidth, asset.PixelHeight);

            ImageData data = new ImageData
            {
                Size = size,
                AssetId = AssetId,
                ImageSize = new Size(size.Width, size.Height)
            };
            ImageElement image = new ImageElement(data);
            ElementId = image.Id;

            return image;
        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, ElementId ?? Guid.Empty);
    }
}

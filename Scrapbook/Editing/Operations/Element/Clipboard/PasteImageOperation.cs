using Scrapbook.Core.Assets.Importers;
using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Scrapbook.Editing.Operations.Element.Clipboard
{
    internal class PasteImageOperation : BoardOperation
    {
        public override string Description => "Takes valid content from the clipboard and pastes it onto the Canvas.";

        public Guid? AssetId { get; private set; }
        public Guid? ElementId { get; private set; }

        private ImageData? _imageData;
        private readonly BitmapEncoder _encoder;
        private readonly string _mediaType;

        public PasteImageOperation(BitmapEncoder encoder, string mediaType) : base(DocumentChangeEvent.ElementAdded)
        {
            _encoder = encoder;
            _mediaType = mediaType;
        }
        public PasteImageOperation(int quality = 100) : base(DocumentChangeEvent.ElementAdded)
        {
            _encoder = new JpegBitmapEncoder()
            {
                QualityLevel = quality
            };
            _mediaType = "image/jpeg";
        }


        public override async Task Execute(BoardDocument document)
        {
            BitmapSource? data = System.Windows.Clipboard.GetImage();

            if (data != null)
            {
                using (MemoryStream byteStream = new MemoryStream())
                {
                    BitmapFrame frame = BitmapFrame.Create(data);
                    _encoder.Frames.Add(frame);
                    _encoder.Save(byteStream);
                    byte[] bit = byteStream.ToArray();

                    ImageAsset imageAsset = new ImageAsset
                    {
                        Name = Guid.NewGuid().ToString(),
                        MediaType = _mediaType,
                        PixelWidth = frame.PixelWidth,
                        PixelHeight = frame.PixelHeight,
                        Content = bit
                    };

                    document.Assets.Add(imageAsset);
                    AssetId = imageAsset.Id;

                    Size2D size = new Size2D(imageAsset.PixelWidth, imageAsset.PixelHeight);
                    _imageData = new ImageData
                    {
                        Size = size,
                        AssetId = (Guid) AssetId,
                        ImageSize = new System.Windows.Size(size.Width, size.Height)
                    };
                    ImageElement imageElement = new ImageElement(_imageData);

                    document.Elements.Add(imageElement);
                    ElementId = imageElement.Id;
                    byteStream.Close();

                    _encoder.Frames.Clear();
                }
            }
            else
            {
                AddToQueue = false;
            }
        }

        public override Task Undo(BoardDocument document)
        {
            if (AssetId != null)
                document.Assets.Trash((Guid) AssetId);

            if (ElementId != null)
                BoardEditorSession.Instance.Editor.RemoveElement((Guid) ElementId);

            return Task.CompletedTask;
        }

        public override Task Redo(BoardDocument document)
        {
            if (_imageData == null || AssetId == null || !document.Assets.Restore((Guid) AssetId))
                return Task.CompletedTask;

            ImageElement imageElement = new ImageElement(_imageData);

            document.Elements.Add(imageElement);
            ElementId = imageElement.Id;

            return Task.CompletedTask;
        }

        public override void CleanUp(BoardDocument document)
        {
            if (AssetId == null)
                return;

            document.Assets.Trash((Guid) AssetId);
        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(DocumentChangeEvent.ElementAdded, AssetId, ElementId);
    }
}

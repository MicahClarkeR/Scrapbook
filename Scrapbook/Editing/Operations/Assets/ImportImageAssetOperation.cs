using Microsoft.Win32;
using Scrapbook.Core.Assets.Importers;
using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.Operations.Interfaces;
using System.IO;
using System.Windows;

namespace Scrapbook.Editing.Operations.Assets
{
    public sealed class ImportImageAssetOperation : BoardOperation
    {
        public override string Description => "";
        public Guid? AssetId { get; private set; }
        public Point Position { get; private set; }

        public string? FilePath { get; private set; }

        public ImportImageAssetOperation(Point position) : base(Events.DocumentChangeEvent.ElementAdded)
        {
            Position = position;
        }

        public override async Task Execute(BoardDocument document)
        {
            if (FilePath == null)
            {
                var fileSelector = new OpenFileDialog()
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                    Multiselect = false
                };

                if (!(fileSelector.ShowDialog() ?? false))
                {
                    throw new Exception("No file selected.");
                }

                FilePath = fileSelector.FileName;
            }

            await InsertImage(document);
        }

        private async Task InsertImage(BoardDocument document)
        {
            if (FilePath == null)
                return;

            using var stream = File.OpenRead(FilePath);
            ImageImporter importer = new ImageImporter();
            ImageAsset image = await importer.ReadAsync(stream, FilePath) ?? throw new Exception("Null image asset returned.");

            document.Assets.Add(image);
            AssetId = image.Id;
        }

        public override async Task Undo(BoardDocument document)
        {
            if (AssetId == null)
                return;

            document.Assets.Trash((Guid) AssetId);

        }

        protected override DocumentChangeEventArgs GetChangeEventArgs() => new DocumentChangeEventArgs(EventType, AssetId);
    }
}

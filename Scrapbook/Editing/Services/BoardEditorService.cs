using Microsoft.Win32;
using Scrapbook.Core.Assets;
using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Events;
using Scrapbook.Editing.History;
using Scrapbook.Editing.Operations.Assets;
using Scrapbook.Editing.Operations.Element;
using Scrapbook.Editing.Operations.Element.Clipboard;
using Scrapbook.Editing.Operations.Interfaces;
using Scrapbook.Editing.Selection;
using Scrapbook.Editing.Services.SubServices;
using Scrapbook.Persistence;
using System.Numerics;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Scrapbook.Editing.Services
{
    // Public API for changing the board.
    public sealed class BoardEditorService : BoardService
    {
        public readonly ElementSubService Elements;

        public BoardEditorService() : base()
        {
            Elements = new ElementSubService();
        }

        public async Task PasteImageAsync()
        {
            var operation = new PasteImageOperation();

            await History.Execute(operation, Document);

            var args = new DocumentChangeEventArgs(DocumentChangeEvent.ElementAdded, (operation.ElementId != null) ? [(Guid)operation.ElementId] : []);
            NotifyDocumentChanged(args);
        }

        public async Task ImportImageAsync(bool select = true)
        {
            var operation = new ImportImageElementOperation();

            await History.Execute(operation, Document);

            var args = new DocumentChangeEventArgs(DocumentChangeEvent.ElementAdded, (operation.ElementId != null) ? [(Guid)operation.ElementId] : []);
            NotifyDocumentChanged(args);

            if (select && operation.ElementId is not null)
            {
                Selection.Clear();
                Selection.Add((Guid)operation.ElementId);
            }
        }

        public void RemoveElement(Guid id)
        {
            var args = new DocumentChangeEventArgs(DocumentChangeEvent.ElementRemoved, id);
            NotifyDocumentChanged(args);
        }

        public void RemoveElement(BoardElement element)
        {
            RemoveElement(element.Id); ;
        }

        public void AddImage(ImageElement element, ImageAsset asset)
        {
            AddImages([element], [asset]);
        }

        public void AddImages(ImageElement[] images, ImageAsset[] assets)
        {
            Guid[] guids = new Guid[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                ImageElement element = images[i];
                ImageAsset asset = assets[i];

                Document.Elements.Add(element);
                Document.Assets.Add(asset);

                guids[i] = element.Id;
            }

            var args = new DocumentChangeEventArgs(DocumentChangeEvent.ElementAdded, guids);
            NotifyDocumentChanged(args);
        }

        public void OpenFile(BoardEditorSession session)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Scrapbook Files (*.sbook)|*.sbook"
            };

            if (dialog.ShowDialog() ?? false)
            {
                DocumentLoader loader = new DocumentLoader(dialog.FileName);
                loader.Load(this, session);
            }
        }
    }
}
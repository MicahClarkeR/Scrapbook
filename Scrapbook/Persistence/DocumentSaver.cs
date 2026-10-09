using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements;
using Scrapbook.Editing;
using Scrapbook.Persistence.DirectoryWrappers;
using Scrapbook.Persistence.Files;

namespace Scrapbook.Persistence
{
    public class BoardSaver
    {
        public readonly BoardEditorSession Session;

        public BoardSaver(BoardEditorSession session)
        {
            Session = session;
        }

        public void Save(string path)
        {
            Zipper zip = new Zipper();
            BoardFile boardFile = new BoardFile(Session.Document);

            SaveAssets(zip);
            boardFile.Save(zip);

            zip.Zip(path);
        }

        private void SaveAssets(Zipper zip)
        {
            var assets = zip.CreateDirectory("assets");
            var imageDirectory = assets.CreateAndWrapDirectory<ZipperDirectory, ImagesWrapper>("images") ?? throw new Exception("Cannot save, Image Directory returned null.");

            SaveImages(imageDirectory);
        }

        private void SaveImages(ImagesWrapper images)
        {
            foreach (var image in Session.Document.Elements.GetAll<ImageElement>())
            {
                var asset = Session.Document.Assets.Get<ImageAsset>(image.AssetId);

                if (asset != null)
                    images.Add(image, asset);
            }

            images.PrepareToSave();
        }
    }
}

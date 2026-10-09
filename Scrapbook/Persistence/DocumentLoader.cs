using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Editing;
using Scrapbook.Editing.Services;
using Scrapbook.Helper;
using Scrapbook.Persistence.DirectoryWrappers;
using Scrapbook.Persistence.Files;
using Scrapbook.Persistence.SpecialDirectories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static Scrapbook.Persistence.Files.BoardFile;
using static Scrapbook.Persistence.Files.ImagesMetadataFile;

namespace Scrapbook.Persistence
{
    public class DocumentLoader
    {
        private readonly ZipBrowser _browser;

        private ImageElement[] ImageElements = new ImageElement[0];
        private ImageAsset[] ImageAssets = new ImageAsset[0];

        public DocumentLoader(string path)
        {
            _browser = new ZipBrowser(path);
        }

        public void Load(BoardEditorService service, BoardEditorSession session)
        {
            var boardFileArchive = _browser.Root.GetFile(BoardFile.FileName);
            var boardFile = new BoardFile(boardFileArchive.Data);

            session.Document = boardFile.Document;

            LoadAssets(service, session);
        }

        private void LoadAssets(BoardEditorService service, BoardEditorSession session)
        {
            var assetData = _browser.Root.GetDirectory("assets");
            ImagesWrapper images = assetData.WrapDirectory<ImagesWrapper>("images");
            Guid[] guids = images.GetGuids().ToArray();
            int size = guids.Count();
            ImageElements = new ImageElement[size];
            ImageAssets = new ImageAsset[size];

            for (int i = 0; i < size; i++)
            {
                Guid id = guids[i];
                ImageData elementData = images.GetElementData(id);
                ImageAsset asset = images.GetAssetData(id);
                ZipperFile file = images.Directory.GetFile(asset.Id);
                asset.Content = file.Data;

                ImageElement element = new ImageElement(elementData);

                ImageElements[i] = element;
                ImageAssets[i] = asset;
            }

            service.AddImages(ImageElements, ImageAssets);
        }
    }
}

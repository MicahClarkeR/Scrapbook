using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Documents;
using Scrapbook.Core.Elements;
using Scrapbook.Core.Geometry;
using Scrapbook.Helper;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Scrapbook.Persistence.Files
{
    public class BoardFile : ScrapFile
    {
        public static string FileName => "document.xml";

        public readonly BoardDocument Document;

        public List<ImageAsset> ImageAssets => Document.Assets.Get<ImageAsset>();

        private BoardFileContent _loadedContent;

        public BoardFile(BoardDocument document) : base()
        {
            Document = document;
        }

        public BoardFile(byte[] data)
        {
            Load(data);

            Document = new BoardDocument()
            {
                SchemaVersion = _loadedContent.SchemaVersion,
                Id = _loadedContent.Id,
                Name = _loadedContent.Name,
                Canvas = _loadedContent.Canvas,
            };
        }

        public override void Save(Zipper zip)
        {
            BoardFileContent content = new BoardFileContent()
            {
                SchemaVersion = Document.SchemaVersion,
                Id = Document.Id,
                Name = Document.Name,
                Canvas = Document.Canvas,
            };

            string documentXml = XmlHelper.ToXml(content);
            zip.Add(FileName, new ZipperFile(FileName, documentXml));
        }

        public override void Load(byte[] data)
        {
            _loadedContent = XmlHelper.FromXml<BoardFileContent>(data);
        }


        public struct BoardFileContent
        {
            public int SchemaVersion;
            public Guid Id;
            public string Name;
            public CanvasDefinition Canvas;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scrapbook.Persistence.Files
{
    public interface IScrapFile
    {
        public void Save(Zipper zip);
        public void Load(byte[] data);
    }

    public abstract class ScrapFile : IScrapFile
    {

        public ScrapFile()
        {
        }

        public abstract void Save(Zipper zip);
        public abstract void Load(byte[] data);
    }
}

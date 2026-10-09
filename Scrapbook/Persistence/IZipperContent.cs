namespace Scrapbook.Persistence
{
    public interface IZipperContent
    {
        string Name { get; }

        public void PrepareToSave();
    }
}

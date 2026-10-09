using ComputeSharp;
using ComputeSharp.Resources;
using Scrapbook.Shading.Elements;
using Scrapbook.Shading.Shaders.Processors;
using Scrapbook2.Tests.Assets;

namespace Scrapbook2.Tests
{
    public class ShaderTests
    {
        private const string BallsFilename = "balls.jpg";
        private static FileInfo TestFile => AssetHelper.GetAssetInfo(BallsFilename);

        [Fact]
        public void BasicImageShadingTest()
        {
            Stream fileStream = AssetHelper.GetFileStream(FileMode.Open, BallsFilename);
            ShadedImage<GrayscaleEffectProcessor> image = new ShadedImage<GrayscaleEffectProcessor>(900, 600, fileStream);
            image.Apply();
            Texture2D<Rgba32> shadedTexture = image.ShadedTexture
                ?? throw new Exception("Shaded iamge returned 'null'");

            string savePath = AssetHelper.GetAssetPath("balls-result.jpeg");
            FileStream saveStream = new FileStream(savePath, FileMode.Create);
            shadedTexture.Save(saveStream, ImageFormat.Jpeg);
        }
    }
}

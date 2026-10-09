using Scrapbook.Core.Assets.Types;
using Scrapbook.Core.Elements.Data;
using Scrapbook.Core.Geometry;
using Scrapbook.Core.Properties;
using Scrapbook.Core.Properties.Interaction;
using Scrapbook.Core.Properties.Values;
using Scrapbook.Editing;
using Scrapbook.Editing.Services;
using Scrapbook.UI.Board;
using Scrapbook.UI.Selection.Properties.Controls.Interaction.Args;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Scrapbook.Core.Elements
{
    public class ImageElement : SizedElement
    {
        /// <summary>
        /// ID of the underlying Asset being displayed by this Element.
        /// </summary>
        public Guid AssetId { get => Data.AssetId; set => Data.AssetId = value; }

        /// <summary>
        /// Dictates if this Element should maintain aspect ratio when scaled.
        /// </summary>
        public bool MaintainAspectRatio { get => Data.MaintainAspectRatio; set => Data.MaintainAspectRatio = value; }

        /// <summary>
        /// Raw dimensions of the underlying Image.
        /// </summary>
        public Size ImageSize { get => Data.ImageSize; set => Data.ImageSize = value; }

        /// <summary>
        /// Used to dictate the cropping of this Image Element for display. <br /> 
        /// <b><i>Currently unimplemented.</i></b>
        /// </summary>
        public Rect2D? SourceCrop { get => Data.SourceCrop; set => Data.SourceCrop = value; }

        /// <summary>
        /// The underlying Image Data used by this Element.
        /// </summary>
        protected ImageData Data = new ImageData();

        /// <summary>
        /// Image Control that will be displayed on the Board Canvas.
        /// </summary>
        private Image? _image;
        
        public ImageElement(ImageData data)
        {
            Data = data;
        }

        [Obsolete("Provide ImageData or use blank constructor.")]
        public ImageElement(Guid assetId, Size imageSize)
        {
            AssetId = assetId;
            ImageSize = imageSize;
        }

        protected override PropertyDictionary? GetElementProperties(BoardEditorService service)
        {
            var dict = new PropertyDictionary
            {
                {
                    "Position",
                    new PointValue(Position, async (s, e) =>
                    {
                        var pos = (System.Windows.Point)e.NewValue;
                        await service.Elements.SetElementPositionAsync(pos, Id);
                    })
                },
                {
                    "Size",
                    new SizeValue(Size, async (s, e) =>
                    {
                        var size = (Size2D)e.NewValue;
                        UpdateSize(size);
                        await service.Elements.SetElementSizeAsync(size, Id);
                    })
                },
                {
                    "Maintain Aspect Ratio",
                    new BooleanValue(MaintainAspectRatio, async (s, e) =>
                    {
                        var maintain = (bool)e.NewValue;
                        await service.Elements.Images.SetMaintainAspectRatioAsync(maintain, Id);
                    })
                },
                {
                    "Rotation",
                    new IntValue(Rotation, async (s, e) =>
                    {
                        var rotation = (IntValue)e.NewValue;
                        await service.Elements.SetElementRotationAsync(rotation.Value, Id);
                    })
                },
                {
                    "Reset Size",
                    new ButtonProperty("Reset Size", async (s, e) => await ResetSizeAsync(service, e))
                }
            };

            return dict;
        }

        /// <summary>
        /// Reset the size of this Image Elemnt to the default size of the underlying Image.
        /// </summary>
        /// <param name="service">Service to use when setting the size.</param>
        /// <param name="args"></param>
        /// <returns>Task for completion.</returns>
        public async Task ResetSizeAsync(BoardEditorService service, ButtonClickArgs args)
        {
            Size = new Size2D((float) ImageSize.Width, (float) ImageSize.Height);
            await service.Elements.SetElementSizeAsync(Size, Id);
        }

        /// <summary>
        /// Set the size of this Image Element, using aspect ratio if set.
        /// </summary>
        /// <param name="size">Desired size of the Element.</param>
        private void UpdateSize(Size2D size)
        {
            if (MaintainAspectRatio)
            {
                if (size.Width != Size.Width)
                    size.Height *= (size.Width / Size.Width);
                else if (size.Height != Size.Height)
                    size.Width *= (size.Height / Size.Height);
            }

            Size = size;
        }

        /// <summary>
        /// Get this Image Element's Image Data.
        /// </summary>
        /// <returns>The Image Data for this Element.</returns>
        public override ImageData GetData() => Data;

        public override bool AddToCanvas(BoardCanvas canvas, BoardEditorSession session)
        {
            ImageAsset? asset = session.Assets.Get<ImageAsset>(AssetId);

            if (asset == null)
                return false;

            BitmapImage bitmap = new BitmapImage();
            MemoryStream stream = new MemoryStream(asset.Content);

            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.EndInit();

            _image = new Image()
            {
                Source = bitmap,
                DataContext = this,
                Stretch = System.Windows.Media.Stretch.Fill,
                Width = Size.Width,
                Height = Size.Height,
                RenderTransformOrigin = new Point(Size.Width / 2, Size.Height / 2)
            };

            _image.MouseLeftButtonUp += (s, e) =>
            {
                session.Selection.Clear();
                session.Selection.Add(Id);
            };
            _image.RenderTransform = new TransformGroup();

            canvas.Children.Add(_image);

            return true;
        }

        public override FrameworkElement? GetElement() => _image;
    }
}

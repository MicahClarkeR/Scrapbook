using Scrapbook.Core.Geometry;
using Scrapbook.Editing.Operations.Element;
using System.Numerics;
using System.Windows;

namespace Scrapbook.Editing.Services.SubServices
{
    public class ElementSubService : BoardService, ISubService
    {
        public readonly ImagesSubService Images;

        public ElementSubService() : base()
        {
            Images = new ImagesSubService();
        }

        public async Task RenameElementAsync(string name, Guid elementId)
        {
            var operation = new RenameElementOperation(name, elementId);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetOpacityAsync(double opacity, params Guid[] elementIds)
        {
            var operation = new SetElementOpacityOperation(opacity, elementIds);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetVisibilityAsync(bool visible, params Guid[] elementIds)
        {
            var operation = new SetElementVisibilityOperation(visible, elementIds);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetElementLockedAsync(bool locked, params Guid[] elementIds)
        {
            var operation = new SetElementLockedOperation(locked, elementIds);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetElementPositionAsync(Point position, Guid elementId)
        {
            var operation = new SetElementPositionOperation(position, elementId);
            await ExecuteOperationAsync(operation);
        }

        public async Task MoveElementAsync(Vector2 distance, Guid elementId)
        {
            var operation = new MoveElementOperation(distance, elementId);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetElementRotationAsync(int rotation, Guid elementId)
        {
            var operation = new SetElementRotationOperation(rotation, elementId);
            await ExecuteOperationAsync(operation);
        }

        public async Task SetElementSizeAsync(Size2D size, Guid elementId)
        {
            var operation = new SetElementSizeOperation(size, elementId);
            await ExecuteOperationAsync(operation);
        }

        public void SelectElement(Guid elementId)
        {
            Selection.Clear();
            Selection.Add(elementId);
        }
    }
}

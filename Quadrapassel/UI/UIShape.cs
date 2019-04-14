using Quadrapassel.UI.Abstraction;
using SFML.Graphics;

namespace Quadrapassel.UI
{
    public class UIShape : UIElement
    {
        private readonly Shape _shape;

        private readonly int _transpositionX;
        private readonly int _transpositionY;

        public UIShape(Shape shape)
        {
            _shape = shape;
        }

        public UIShape(Shape shape, int transX, int transY) : this(shape)
        {
            _transpositionX = transX;
            _transpositionY = transY;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            //if (IsVisible)
            //    target.Draw(_body, states);

            DrawBlocks(target, states);
        }

        private void DrawBlocks(RenderTarget target, RenderStates states)
        {
            if (_shape != null)
            {
                foreach (var shapeBlock in _shape.Blocks)
                {
                    var block = new UIBlock(shapeBlock, _shape.X * UIBlock.Size + _transpositionX, _shape.Y * UIBlock.Size + _transpositionY);
                    block.Draw(target, states);
                }
            }
        }
    }
}

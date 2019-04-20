using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIBlock : UIElement
    {
        public static int Size = 40;

        private static int Width => Size;
        private static int Height => Size;

        private readonly int _transpositionX;
        private readonly int _transpositionY;

        private readonly Block _blockModel;

        private RectangleShape _body;

        public int PositionX => _blockModel?.X ?? 0;
        public int PositionY => _blockModel?.Y ?? 0;
        public bool IsVisible { get; set; } 
        
        public UIBlock(Block blockModel)
        {
            _blockModel = blockModel;
            IsVisible = true;
        }

        public UIBlock(Block blockModel, int transX, int transY) : this(blockModel)
        {
            _transpositionX = transX;
            _transpositionY = transY;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_body, states);
        }

        private void UpdateBody()
        {
            _body = new RectangleShape
            {
                Position = new Vector2f(PositionX * Size + _transpositionX, PositionY * Size + _transpositionY),
                Size = new Vector2f(Width, Height),
                FillColor = ColorConverter.Convert(_blockModel.Color)
            };
        }
    }
}

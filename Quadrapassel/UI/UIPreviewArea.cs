using System.Linq;
using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIPreviewArea : UIElement
    {
        private readonly Game _gameModel;

        public int Width => 5 * UIBlock.Size;
        public int Height => 5 * UIBlock.Size;

        //private Sprite _body;
        private RectangleShape _body;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public UIPreviewArea(Game gameModel)
        {
            _gameModel = gameModel;
            IsVisible = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_body, states);

            DrawBlock(target, states);
        }

        private void DrawBlock(RenderTarget target, RenderStates states)
        {
            if (_gameModel.NextShape != null)
            {
                var shapeWidth = _gameModel.NextShape.Blocks.Max(b => b.X) - _gameModel.NextShape.Blocks.Min(b => b.X) + 1;
                var shapeHeight = _gameModel.NextShape.Blocks.Max(b => b.Y) - _gameModel.NextShape.Blocks.Min(b => b.Y) + 1;
                var shape = new UIShape(_gameModel.NextShape, 
                    PositionX + (Width  - shapeWidth  * UIBlock.Size) / 2 - (_gameModel.NextShape.X + _gameModel.NextShape.Blocks.Min(b => b.X) ) * UIBlock.Size,
                    PositionY + (Height - shapeHeight * UIBlock.Size) / 2 - (_gameModel.NextShape.Y + _gameModel.NextShape.Blocks.Min(b => b.Y) ) * UIBlock.Size);
                shape.Draw(target, states);
            }
        }

        private void UpdateBody()
        {
            _body = new RectangleShape
            {
                Position = new Vector2f(PositionX, PositionY),
                Size = new Vector2f(Width, Height),
                FillColor = Color.Black
            };
        }
    }
}

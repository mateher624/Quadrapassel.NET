using System.Linq;
using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIPreviewArea : UIElement
    {
        private readonly GameController _gameController;

        public int Width => 5 * UIBlock.Size;
        public int Height => 5 * UIBlock.Size;

        private RectangleShape _body;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public UIPreviewArea(GameController gameController)
        {
            _gameController = gameController;
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
            if (_gameController.Game != null)
            {
                var nextShape = _gameController.Game.NextShape;
                if (nextShape != null)
                {
                    var shapeWidth = nextShape.Blocks.Max(b => b.X) - nextShape.Blocks.Min(b => b.X) + 1;
                    var shapeHeight = nextShape.Blocks.Max(b => b.Y) - nextShape.Blocks.Min(b => b.Y) + 1;
                    var shape = new UIShape(nextShape,
                        PositionX + (Width - shapeWidth * UIBlock.Size) / 2 - (nextShape.X + nextShape.Blocks.Min(b => b.X)) * UIBlock.Size,
                        PositionY + (Height - shapeHeight * UIBlock.Size) / 2 - (nextShape.Y + nextShape.Blocks.Min(b => b.Y)) * UIBlock.Size);
                    shape.Draw(target, states);
                }
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

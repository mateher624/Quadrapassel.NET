using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIGameArea : UIElement
    {
        private readonly GameController _gameController;

        public int Width => _gameController.Width * UIBlock.Size;
        public int Height => _gameController.Height * UIBlock.Size;

        //private Sprite _body;
        private RectangleShape _body;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public UIGameArea(GameController gameController)
        {
            _gameController = gameController;
            IsVisible = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_body, states);

            DrawBlocks(target, states);
        }

        private void DrawBlocks(RenderTarget target, RenderStates states)
        {
            if (_gameController.Game != null)
            {
                for (var i = 0; i < _gameController.Game.Width; i++)
                {
                    for (var j = 0; j < _gameController.Game.Height; j++)
                    {
                        if (_gameController.Game.Blocks[i, j] != null)
                        {
                            var block = new UIBlock(_gameController.Game.Blocks[i, j], PositionX, PositionY);
                            block.Draw(target, states);
                        }
                    }
                }

                if (_gameController.Game?.Shape != null)
                {
                    var shape = new UIShape(_gameController.Game.Shape, PositionX, PositionY);
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

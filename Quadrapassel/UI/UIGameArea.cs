using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIGameArea : IUIElement
    {
        private readonly Game _gameModel;

        public int Width => _gameModel.Width * UIBlock.Size;
        public int Height => _gameModel.Height * UIBlock.Size;

        //private Sprite _body;
        private RectangleShape _body;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public UIGameArea(Game gameModel)
        {
            _gameModel = gameModel;
            IsVisible = true;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_body, states);

            DrawBlocks(target, states);
        }

        private void DrawBlocks(RenderTarget target, RenderStates states)
        {
            for (var i = 0; i < _gameModel.Width; i++)
            {
                for (var j = 0; j < _gameModel.Height; j++)
                {
                    if (_gameModel.Blocks[i, j] != null)
                    {
                        var block = new UIBlock(_gameModel.Blocks[i, j], PositionX, PositionY);
                        block.Draw(target, states);
                    }
                }
            }

            if (_gameModel.Shape != null)
            {
                var shape = new UIShape(_gameModel.Shape, PositionX, PositionY);
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

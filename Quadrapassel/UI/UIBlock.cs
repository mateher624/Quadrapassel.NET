using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIBlock : IUIElement
    {
        public static int Size = 40;

        private int _width => Size;
        private int _height => Size;

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

        public void Draw(RenderTarget target, RenderStates states)
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
                Size = new Vector2f(_width, _height),
                FillColor = ColorConverter.Convert(_blockModel.Color)
            };
        }
    }
}

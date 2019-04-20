using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UILabel : UIElement
    {
        private static readonly Font font = new Font("./Resources/arial.ttf");
        private Text _text;

        public int Width { get; set; }
        public int Height { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public string Caption { get; set; }
        public int Size { get; set; } = UIBlock.Size;
        public int FontSize { get; set; } = (int)(UIBlock.Size * 0.75);

        public Color TextColor { get; set; } = Color.White;
        public Color OutlineColor { get; set; } = Color.Black;
        public int OutlineThickness { get; set; } = 2;
        

        public UILabel()
        {
            IsVisible = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_text, states);
        }

        private void UpdateBody()
        {
            _text = new Text
            {
                Font = font,
                Origin = new Vector2f(Size, Size),
                Position = new Vector2f(PositionX + Width/2, PositionY + Height/2),
                DisplayedString = Caption,
                CharacterSize = (uint)FontSize,
                OutlineThickness = OutlineThickness,
                OutlineColor = OutlineColor,
                FillColor = TextColor
            };
            var bounds = _text.GetLocalBounds();
            _text.Origin = new Vector2f(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
        }
    }
}

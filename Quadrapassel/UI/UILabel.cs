using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UILabel : IUIElement
    {
        private static readonly Font Font = new Font("./Resources/arial.ttf");
        private Text _text;

        public int Width { get; set; }
        public int Height { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public string Caption { get; set; }

        public UILabel()
        {
            IsVisible = true;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
                target.Draw(_text, states);
        }

        private void UpdateBody()
        {
            _text = new Text
            {
                Font = Font,
                Origin = new Vector2f(40, 40),
                Position = new Vector2f(PositionX + Width/2, PositionY + Height/2),
                DisplayedString = Caption,
                FillColor = Color.Black
            };
            var bounds = _text.GetLocalBounds();
            _text.Origin = new Vector2f(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);
        }
    }
}

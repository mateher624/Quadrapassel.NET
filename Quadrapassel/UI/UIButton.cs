using System;
using Quadrapassel.UI.Abstraction;
using Quadrapassel.UI.States;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Quadrapassel.UI
{
    public class UIButton : UIElement, ICollidable, IClickable
    {
        private static readonly Font font = new Font("./Resources/arial.ttf");

        private ButtonState _state;
        private RectangleShape _body;
        private Text _text;

        public int Width { get; set; }
        public int Height { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public string Caption { get; set; }

        public Action Action { get; set; }

        public UIButton()
        {
            IsVisible = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
            {
                target.Draw(_body, states);
                target.Draw(_text, states);
            } 
        }

        public void Collide(int x, int y)
        {
            if (_state == ButtonState.Disabled)
                return;

            var isCollision = x >= PositionX && x <= PositionX + Width && y >= PositionY && y <= PositionY + Height;
            if (isCollision)
            {
                if (_state == ButtonState.Enabled || _state == ButtonState.Hovered)
                    _state = ButtonState.Hovered;
            }
            else
            {
                if (_state == ButtonState.Hovered)
                    _state = ButtonState.Enabled;
            }
        }

        public void Clicked(int x, int y, Mouse.Button button)
        {
            if (_state == ButtonState.Disabled)
                return;

            var isCollision = x >= PositionX && x <= PositionX + Width && y >= PositionY && y <= PositionY + Height;
            if (isCollision)
            {
                if (_state == ButtonState.Hovered || _state == ButtonState.Enabled)
                    _state = ButtonState.Clicked;
            }
        }

        public void Unclicked(int x, int y, Mouse.Button button)
        {
            if (_state == ButtonState.Disabled)
                return;

            var isCollision = x >= PositionX && x <= PositionX + Width && y >= PositionY && y <= PositionY + Height;
            if (isCollision)
            {
                if (_state == ButtonState.Clicked)
                    _state = ButtonState.Hovered;
                Action.Invoke();
            }
            else
            {
                if (_state == ButtonState.Clicked)
                    _state = ButtonState.Enabled;
            }
        }

        private void UpdateBody()
        {
            _body = new RectangleShape
            {
                Position = new Vector2f(PositionX, PositionY),
                Size = new Vector2f(Width, Height),
            };

            _text = new Text
            {
                Font = font,
                Origin = new Vector2f(UIBlock.Size, UIBlock.Size),
                Position = new Vector2f(PositionX + Width / 2, PositionY + Height / 2),
                DisplayedString = Caption,
                FillColor = Color.Black
            };
            var bounds = _text.GetLocalBounds();
            _text.Origin = new Vector2f(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2);

            switch (_state)
            {
                case ButtonState.Enabled:
                    _body.FillColor = Color.Red;
                    break;
                case ButtonState.Disabled:
                    _body.FillColor = Color.Black;
                    break;
                case ButtonState.Hovered:
                    _body.FillColor = Color.Yellow;
                    break;
                case ButtonState.Clicked:
                    _body.FillColor = Color.Green;
                    break;
            }
        } 
    }
}

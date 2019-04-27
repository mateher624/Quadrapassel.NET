using System.Collections.Generic;
using System.Linq;
using Quadrapassel.UI.Abstraction;
using SFML.Graphics;
using SFML.System;

namespace Quadrapassel.UI
{
    public class UIScoresPresenter : UIElement
    {
        private readonly HighScoresTable _highScoresTable;

        public int Width { get; set; }
        public int Height { get; set; }

        private RectangleShape _body;

        private UILabel _title;
        private List<UILabel> _scoresList;

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsVisible { get; set; }

        public UIScoresPresenter(HighScoresTable highScoresTable)
        {
            _highScoresTable = highScoresTable;
            IsVisible = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateBody();
            if (IsVisible)
            {
                target.Draw(_body, states);
                target.Draw(_title, states);
                foreach (var uiLabel in _scoresList)
                {
                    target.Draw(uiLabel, states);
                }
            }
        }

        private void UpdateBody()
        {
            _body = new RectangleShape
            {
                Position = new Vector2f(PositionX, PositionY),
                Size = new Vector2f(Width, Height),
                FillColor = new Color(34, 34, 34, 212)
            };

            _title = new UILabel
            {
                PositionX = PositionX + UIBlock.Size / 2,
                PositionY = PositionY + UIBlock.Size / 2,
                Width = Width - UIBlock.Size,
                Height = UIBlock.Size,
                OutlineThickness = 0,
                Caption = "High Scores"
            };

            var scoresList = new List<UILabel>();
            var scores = _highScoresTable.HighScoresList.OrderByDescending(s => s.Score).Where(s => s.Score != 0).ToList();
            for (var i = 0; i < 10; i++)
            {
                if (i >= scores.Count)
                    break;

                scoresList.Add(new UILabel
                {
                    PositionX = PositionX + UIBlock.Size / 2,
                    PositionY = PositionY + UIBlock.Size / 2 + (i+2) * UIBlock.Size,
                    Width = Width - UIBlock.Size,
                    Height = UIBlock.Size,
                    OutlineThickness = 0,
                    Caption = $"{i+1}. {scores[i].Score.ToString()}"
                });
            }

            _scoresList = scoresList;
        }
    }
}

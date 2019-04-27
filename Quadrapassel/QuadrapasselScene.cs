using System.Collections.Generic;
using Quadrapassel.UI;
using Quadrapassel.UI.Themes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Quadrapassel
{
    public class QuadrapasselScene : Scene
    {
        private readonly GameController _gameController;
        private readonly HighScoresTable _highScoresTable;

        private readonly UIGameArea _gameArea;
        private readonly UIPreviewArea _previewArea;
        private readonly UILabel _scoreNameLabel;
        private readonly UILabel _scoreLabel;
        private readonly UILabel _linesNameLabel;
        private readonly UILabel _linesLabel;
        private readonly UILabel _levelNameLabel;
        private readonly UILabel _levelLabel;
        private readonly UIButton _startPauseButton;
        private readonly UIButton _scoresButton;

        private readonly UIScoresPresenter _scoresPresenter;

        private readonly UILabel _stateLabel;

        public QuadrapasselScene(Settings settings) : base(settings)
        {
            _gameController = new GameController(settings.GameSettings);
            _highScoresTable = HighScoresTable.LoadScores();

            _gameController.ShapeLanded += ShapeLandedCb;
            _gameController.PauseChanged += PauseChangedCb;
            _gameController.Complete += CompleteCb;
            _gameController.Started += StartedCb;

            _gameController.SetMozaic();

            _gameArea = new UIGameArea(_gameController)
            {
                PositionX = UIBlock.Size,
                PositionY = UIBlock.Size
            };
            _previewArea = new UIPreviewArea(_gameController)
            {
                PositionX = UIBlock.Size + _gameArea.Width + UIBlock.Size,
                PositionY = UIBlock.Size
            };

            _scoresPresenter = new UIScoresPresenter(_highScoresTable)
            {
                PositionX = UIBlock.Size,
                PositionY = UIBlock.Size,
                Width = _gameArea.Width,
                Height = _gameArea.Height,
                IsVisible = false
            };

            _scoreNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 7 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "Score"
            };
            _scoreLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 8 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "0"
            };

            _linesNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 10 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "Lines"
            };
            _linesLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 11 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "0"
            };

            _levelNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 13 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "Level"
            };
            _levelLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 14 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                Caption = "0"
            };
            _startPauseButton = new UIButton
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 16 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 2 * UIBlock.Size,
                Caption = "Start",
                Action = PlayButtonClick
            };
            _scoresButton = new UIButton
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 19 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 2 * UIBlock.Size,
                Caption = "High Scores",
                Action = ShowScores
            };
            _stateLabel = new UILabel
            {
                PositionX = _gameArea.PositionX,
                PositionY = _gameArea.PositionY,
                Width = _gameArea.Width,
                Height = _gameArea.Height,
                FontSize = 2 * UIBlock.Size,
                OutlineThickness = 2,
                Caption = "Test",
                IsVisible = false
            };
        }

        private void NewGame()
        {
            _gameController.Stop();
            _gameController.NewGame();
            _gameController.Start();
            UpdateScore();
        }

        private void ShapeLandedCb(int[] lines, List<Block> lineBlocks)
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreLabel.Caption = _gameController.Score.ToString();
            _linesLabel.Caption = _gameController.NLinesDestroyed.ToString();
            _levelLabel.Caption = _gameController.Level.ToString();
        }

        private void PlayButtonClick()
        {
            _scoresPresenter.IsVisible = false;
            if (_gameController.Ready || _gameController.GameOver)
            {
                NewGame();
                return;
            }

            if (!_gameController.GameOver)
                _gameController.TogglePause();
        }

        private void ShowScores()
        {
            if (_scoresPresenter.IsVisible)
            {
                _scoresPresenter.IsVisible = false;
            }
            else
            {
                _scoresPresenter.IsVisible = true;
                if (!_gameController.Ready && !_gameController.GameOver)
                {
                    if (!_gameController.Paused)
                        _gameController.TogglePause();
                }
            }
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Collide((int)coords.X, (int)coords.Y);
            _scoresButton.Collide((int)coords.X, (int)coords.Y);
        }

        protected override void CheckClick(MouseButtonEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Clicked((int)coords.X, (int)coords.Y, e.Button);
            _scoresButton.Clicked((int)coords.X, (int)coords.Y, e.Button);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Unclicked((int)coords.X, (int)coords.Y, e.Button);
            _scoresButton.Unclicked((int)coords.X, (int)coords.Y, e.Button);
        }

        protected override void CheckKeyPressed(KeyEventArgs e)
        {
            var keyCode = e.Code;

            if (_gameController.Ready)
            {
                // Pressing pause with no game will start a new game.
                if (keyCode == Keyboard.Key.Space)
                {
                    _scoresPresenter.IsVisible = false;
                    NewGame();
                    return;
                }

                return;
            }

            if (keyCode == Keyboard.Key.Space)
            {
                if (!_gameController.GameOver)
                {
                    _gameController.TogglePause();
                    if (_scoresPresenter.IsVisible)
                      _scoresPresenter.IsVisible = false;
                    PauseChangedCb();
                }
                return;
            }

            if (_gameController.Paused)
                return;

            if (keyCode == Keyboard.Key.Left)
            {
                _gameController.MoveLeft();
                return;
            }

            if (keyCode == Keyboard.Key.Right)
            {
                _gameController.MoveRight();
                return;
            }

            if (keyCode == Keyboard.Key.Up)
            {
                //if (_settings.get_boolean("rotate-counter-clock-wise"))
                //    _game.rotate_left();
                //else
                //    _game.rotate_right();
                _gameController.RotateRight();
                return;
            }

            if (keyCode == Keyboard.Key.Down)
            {
                _gameController.SetFastForward(true);
                return;
            }

            if (keyCode == Keyboard.Key.Enter)
            {
                _gameController.Drop();
                return;
            }
        }

        public void CompleteCb()
        {
            if (_gameController.GameOver)
            {
                _stateLabel.Caption = "Game Over";
                _stateLabel.IsVisible = true;
                _startPauseButton.Caption = "New Game";
            }
            else
            {
                _stateLabel.IsVisible = false;
            }

            _highScoresTable.AddScore(_gameController.Score);
            HighScoresTable.SaveScore(_highScoresTable);
            _scoresPresenter.IsVisible = true;
        }

        public void PauseChangedCb()
        {
            if (_gameController.Paused)
            {
                _startPauseButton.Caption = "Resume";
                _stateLabel.Caption = "Paused";
                _stateLabel.IsVisible = true;
            }
            else
            {
                _startPauseButton.Caption = "Pause";
                _stateLabel.IsVisible = false;
            }
        }

        private void StartedCb()
        {
            _stateLabel.IsVisible = false;
        }

        protected override void CheckKeyReleased(KeyEventArgs e)
        {
            var keyCode = e.Code;

            if (_gameController.Ready)
                return;

            if (keyCode == Keyboard.Key.Left || keyCode == Keyboard.Key.Right)
            {
                _gameController.StopMoving();
                return;
            }
            if (keyCode == Keyboard.Key.Down)
            {
                _gameController.SetFastForward(false);
                return;
            }
        }

        protected override void LoadContent()
        {
            //throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            ThemeManager.SetTheme(Theme.Dark);
            ClearColor = ThemeManager.GlobalTheme.BackgroundColor;
        }

        protected override void Tick()
        {
            //throw new NotImplementedException();
        }

        protected override void Render()
        {
            Window.Draw(_gameArea);
            Window.Draw(_previewArea);

            Window.Draw(_scoreNameLabel);
            Window.Draw(_scoreLabel);
            Window.Draw(_linesNameLabel);
            Window.Draw(_linesLabel);
            Window.Draw(_levelNameLabel);
            Window.Draw(_levelLabel);
            Window.Draw(_startPauseButton);
            Window.Draw(_stateLabel);
            Window.Draw(_scoresButton);
            Window.Draw(_scoresPresenter);
        }
    }
}

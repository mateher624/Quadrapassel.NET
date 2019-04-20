using System.Collections.Generic;
using Quadrapassel.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Quadrapassel
{
    public class QuadrapasselScene : Scene
    {
        private readonly Game _game;
        private readonly UIGameArea _gameArea;
        private readonly UIPreviewArea _previewArea;
        private readonly UILabel _scoreNameLabel;
        private readonly UILabel _scoreLabel;
        private readonly UILabel _linesNameLabel;
        private readonly UILabel _linesLabel;
        private readonly UILabel _levelNameLabel;
        private readonly UILabel _levelLabel;
        private readonly UIButton _startPauseButton;

        private readonly UILabel _stateLabel;

        public QuadrapasselScene(Settings settings) : base(settings)
        {
            _game = new Game(
                settings.GameSettings.Lines, 
                settings.GameSettings.Columns, 
                settings.GameSettings.StartingLevel, 
                settings.GameSettings.Lines, 
                10
            );
            _game.ShapeLanded += ShapeLandedCb;
            _game.PauseChanged += PauseChangedCb;
            _game.Complete += CompleteCb;
            _game.Started += StartedCb;

            _gameArea = new UIGameArea(_game)
            {
                PositionX = UIBlock.Size,
                PositionY = UIBlock.Size
            };
            _previewArea = new UIPreviewArea(_game)
            {
                PositionX = UIBlock.Size + _gameArea.Width + UIBlock.Size,
                PositionY = UIBlock.Size
            };

            _scoreNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 7 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
                Caption = "Score"
            };
            _scoreLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 8 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
                Caption = "0"
            };

            _linesNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 10 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
                Caption = "Lines"
            };
            _linesLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 11 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
                Caption = "0"
            };

            _levelNameLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 13 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
                Caption = "Level"
            };
            _levelLabel = new UILabel
            {
                PositionX = 2 * UIBlock.Size + _gameArea.Width,
                PositionY = 14 * UIBlock.Size,
                Width = 5 * UIBlock.Size,
                Height = 1 * UIBlock.Size,
                OutlineThickness = 0,
                TextColor = Color.Black,
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
            _stateLabel = new UILabel
            {
                PositionX = _gameArea.PositionX,
                PositionY = _gameArea.PositionY,
                Width = _gameArea.Width,
                Height = _gameArea.Height,
                FontSize = 2 * UIBlock.Size,
                Caption = "Test",
                IsVisible = false
            };
        }

        private void NewGame()
        {

            _game.Stop();
            //SignalHandler.disconnect_matched(_game, SignalMatchType.DATA, 0, 0, null, null, this);

            //_game = new Game(20, 14, _settings.get_int("starting-level"), _settings.get_int("line-fill-height"), _settings.get_int("line-fill-probability"), _settings.get_boolean("pick-difficult-blocks"));
            //_game.pause_changed.connect(PauseChangedCb);
            //_game.shape_landed.connect(ShapeLandedCb);
            //_game.complete.connect(CompleteCb);
            //_preview.game = _game;
            //_view.game = _game;
            _game.Reset(
                Settings.GameSettings.StartingLevel,
                Settings.GameSettings.FilledLines,
                Settings.GameSettings.FillProb,
                Settings.GameSettings.PickDifficultBlocks
            );

            _game.Start();

            UpdateScore();
            //_pauseAction.set_enabled(true);
            //_pausePlayButton.action_name = "app.pause";
        }

        private void ShapeLandedCb(int[] lines, List<Block> lineBlocks)
        {
            UpdateScore();
        }

        private void UpdateScore()
        {
            _scoreLabel.Caption = _game.Score.ToString();
            _linesLabel.Caption = _game.NLinesDestroyed.ToString();
            _levelLabel.Caption = _game.Level.ToString();
        }

        private void PlayButtonClick()
        {
            if (_game.Ready || _game.GameOver)
            {
                NewGame();
                return;
            }

            if (!_game.GameOver)
                _game.Paused = !_game.Paused;
        }

        protected override void CheckCollide(MouseMoveEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Collide((int)coords.X, (int)coords.Y);
        }

        protected override void CheckClick(MouseButtonEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Clicked((int)coords.X, (int)coords.Y, e.Button);
        }

        protected override void CheckUnClick(MouseButtonEventArgs e)
        {
            var coords = Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            _startPauseButton.Unclicked((int)coords.X, (int)coords.Y, e.Button);
        }

        protected override void CheckKeyPressed(KeyEventArgs e)
        {
            var keyCode = e.Code;

            if (_game.Ready)
            {
                // Pressing pause with no game will start a new game.
                if (keyCode == Keyboard.Key.Space)
                {
                    NewGame();
                    return;
                }

                return;
            }

            if (keyCode == Keyboard.Key.Space)
            {
                if (!_game.GameOver)
                {
                    _game.Paused = !_game.Paused;
                    PauseChangedCb();
                }
                return;
            }

            if (_game.Paused)
                return;

            if (keyCode == Keyboard.Key.Left)
            {
                _game.MoveLeft();
                return;
            }

            if (keyCode == Keyboard.Key.Right)
            {
                _game.MoveRight();
                return;
            }

            if (keyCode == Keyboard.Key.Up)
            {
                //if (_settings.get_boolean("rotate-counter-clock-wise"))
                //    _game.rotate_left();
                //else
                //    _game.rotate_right();
                _game.RotateRight();
                return;
            }

            if (keyCode == Keyboard.Key.Down)
            {
                _game.SetFastForward(true);
                return;
            }

            if (keyCode == Keyboard.Key.Enter)
            {
                _game.Drop();
                return;
            }
        }

        public void CompleteCb()
        {
            if (_game.GameOver)
            {
                _stateLabel.Caption = "Game Over";
                _stateLabel.IsVisible = true;
                _startPauseButton.Caption = "New Game";
            }
            else
            {
                _stateLabel.IsVisible = false;
            }
        }

        public void PauseChangedCb()
        {
            if (_game.Paused)
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

            if (_game.Ready)
                return;

            if (keyCode == Keyboard.Key.Left || keyCode == Keyboard.Key.Right)
            {
                _game.StopMoving();
                return;
            }
            if (keyCode == Keyboard.Key.Down)
            {
                _game.SetFastForward(false);
                return;
            }
        }

        protected override void LoadContent()
        {
            //throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            //throw new NotImplementedException();
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
        }
    }
}

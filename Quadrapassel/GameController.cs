using System.Collections.Generic;

namespace Quadrapassel
{
    public class GameController
    {
        private readonly GameSettings _gameSettings;
        private Game _game;

        public event EventHandler Started;
        public event EventHandler ShapeAdded;
        public event EventHandler ShapeMoved;
        public event EventHandler ShapeDropped;
        public event EventHandler ShapeRotated;
        public event ShapeLandedEventHandler ShapeLanded;
        public event EventHandler PauseChanged;
        public event EventHandler Complete;

        public Game Game => _game;

        public int Width => _game?.Width ?? _gameSettings.Columns;
        public int Height => _game?.Height ?? _gameSettings.Lines;
        public int Score => _game?.Score ?? 0;
        public int NLinesDestroyed => _game?.NLinesDestroyed ?? 0;
        public int Level => _game?.Level ?? 0;
        public bool Ready => _game?.Ready ?? false;
        public bool GameOver => _game?.GameOver ?? false;
        public bool Paused => _game?.Paused ?? false;

        public GameController(GameSettings gameSettings)
        {
            Started += FreeEvent;
            ShapeAdded += FreeEvent;
            ShapeMoved += FreeEvent;
            ShapeDropped += FreeEvent;
            ShapeRotated += FreeEvent;
            ShapeLanded += FreeEvent;
            PauseChanged += FreeEvent;
            Complete += FreeEvent;

            _gameSettings = gameSettings;
        }

        public void SetMozaic()
        {
            _game = new Game(
                _gameSettings.Lines,
                _gameSettings.Columns,
                _gameSettings.StartingLevel,
                _gameSettings.Lines,
                10
            );
            InitializeEvents();
        }

        public void Stop()
        {
            _game?.Stop();
        }

        public void NewGame()
        {
            _game = new Game(
                _gameSettings.Lines,
                _gameSettings.Columns,
                _gameSettings.StartingLevel,
                _gameSettings.FilledLines,
                _gameSettings.FillProb,
                _gameSettings.PickDifficultBlocks
            );
            InitializeEvents();
        }

        public void Start()
        {
            _game?.Start();
        }

        public void Resume()
        {
            if (_game != null)
                _game.Paused = !_game.Paused;
        }

        public void MoveLeft()
        {
            _game?.MoveLeft();
        }

        public void MoveRight()
        {
            _game?.MoveRight();
        }

        public void RotateRight()
        {
            _game?.RotateRight();
        }

        public void RotateLeft()
        {
            _game?.RotateLeft();
        }

        public void SetFastForward(bool b)
        {
            _game?.SetFastForward(b);
        }

        public void Drop()
        {
            _game?.Drop();
        }

        public void StopMoving()
        {
            _game?.StopMoving();
        }

        private void InitializeEvents()
        {
            if (_game != null)
            {
                _game.Started += () => Started();
                _game.ShapeAdded += () => ShapeAdded();
                _game.ShapeMoved += () => ShapeMoved();
                _game.ShapeDropped += () => ShapeDropped();
                _game.ShapeRotated += () => ShapeRotated();
                _game.ShapeLanded += (l, b) => ShapeLanded(l, b);
                _game.PauseChanged += () => PauseChanged();
                _game.Complete += () => Complete();
            }
        }

        public void FreeEvent()
        {
            // do nothing
        }

        private void FreeEvent(int[] lines, List<Block> lineBlocks)
        {
            // do nothing
        }
    }
}

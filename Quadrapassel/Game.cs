using System;
using System.Collections.Generic;

namespace Quadrapassel
{
    public delegate void EventHandler();
    public delegate void ShapeLandedEventHandler(int[] lines, List<Block> lineBlocks);

    public class Game
    {
        private const int NColors = 7;

        private static readonly int[] blockTable = { 
            /* *** */
            /* *   */
            0, 0, 0, 0,
            1, 1, 1, 0,
            1, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,

            0, 0, 1, 0,
            1, 1, 1, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            1, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            /* *** */
            /*   * */
            0, 0, 0, 0,
            1, 1, 1, 0,
            0, 0, 1, 0,
            0, 0, 0, 0,

            0, 1, 1, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            1, 0, 0, 0,
            1, 1, 1, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 0, 0,
            1, 1, 0, 0,
            0, 0, 0, 0,

            /* *** */
            /*  *  */
            0, 0, 0, 0,
            1, 1, 1, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 1, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            1, 1, 1, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            1, 1, 0, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            /*  ** */
            /* **  */

            0, 0, 0, 0,
            0, 1, 1, 0,
            1, 1, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 1, 0,
            0, 0, 1, 0,
            0, 0, 0, 0,

            0, 1, 1, 0,
            1, 1, 0, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            1, 0, 0, 0,
            1, 1, 0, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            /* **  */
            /*  ** */

            0, 0, 0, 0,
            1, 1, 0, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,

            0, 0, 1, 0,
            0, 1, 1, 0,
            0, 1, 0, 0,
            0, 0, 0, 0,

            1, 1, 0, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            1, 1, 0, 0,
            1, 0, 0, 0,
            0, 0, 0, 0,

            /* **** */
            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,

            0, 0, 0, 0,
            1, 1, 1, 1,
            0, 0, 0, 0,
            0, 0, 0, 0,

            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,

            /* ** */
            /* ** */
            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,

            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,

            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0,

            0, 0, 0, 0,
            0, 1, 1, 0,
            0, 1, 1, 0,
            0, 0, 0, 0
        };

        /* Falling shape */
        public Shape Shape;

        /* Next shape to be used */
        public Shape NextShape;

        /* Placed blocks */
        public Block[,] Blocks;

        public int Width => Blocks.GetLength(0);

        public int Height => Blocks.GetLength(1);

        /* Number of lines that have been destroyed */
        public int NLinesDestroyed;

        /* Game score */
        public int Score;

        /* Level play started on */
        private int _startingLevel = 1;

        /* true if should pick difficult blocks to place */
        private bool _pickDifficultBlocks;

        /* The current level */
        public int Level => _startingLevel + NLinesDestroyed / 10;

        /* The direction we are moving */
        private int _fastMoveDirection;

        /* Timer to animate moving fast */
        private EventTimer _fastMoveTimeout;

        /* true if we are in fast forward mode */
        private bool _fastForward;

        /* Timer to animate block drops */
        private EventTimer _dropTimeout;

        /* true if the game has started */
        private bool _hasStarted;

        /* true if games is paused */
        private bool _paused;

        public bool Paused
        {
            get => _paused;
            set
            {
                _paused = value;
                if (_hasStarted && !GameOver)
                    SetupDropTimer();
                PauseChanged();
            }
        }

        /* The y co-ordinate of the shadow of the falling shape */
        public int ShadowY
        {
            get
            {
                if (Shape == null)
                    return 0;

                var d = 0;
                var g = Copy();
                while (g.MoveShape(0, 1, 0))
                    d++;

                return Shape.Y + d;
            }
        }

        public bool GameOver;

        public bool Ready;

        public event EventHandler Started;
        public event EventHandler ShapeAdded;
        public event EventHandler ShapeMoved;
        public event EventHandler ShapeDropped;
        public event EventHandler ShapeRotated;
        public event ShapeLandedEventHandler ShapeLanded;
        public event EventHandler PauseChanged;
        public event EventHandler Complete;

        public void FreeEvent()
        {
            // do nothing
        }

        private void FreeEvent(int[] lines, List<Block> lineBlocks)
        {
            // do nothing
        }

        public Game(int lines = 20, int columns = 14, int startingLevel = 1, int filledLines = 0, int fillProb = 5, bool pickDifficultBlocks = false)
        {
            Started += FreeEvent;
            ShapeAdded += FreeEvent;
            ShapeMoved += FreeEvent;
            ShapeDropped += FreeEvent;
            ShapeRotated += FreeEvent;
            ShapeLanded += FreeEvent;
            PauseChanged += FreeEvent;
            Complete += FreeEvent;


            Blocks = new Block[columns, lines];
            Reset(startingLevel, filledLines, fillProb, pickDifficultBlocks);
        }

        public void Reset(int startingLevel = 1, int filledLines = 0, int fillProb = 5, bool pickDifficultBlocks = false)
        {
            _startingLevel = startingLevel;
            _pickDifficultBlocks = pickDifficultBlocks;

            var random = new Random();

            /* Start with some pre-filled spaces */
            for (var y = 0; y < Height; y++)
            {
                /* Pick at least one column to be empty */
                var blank = random.Next(0, Width);

                for (var x = 0; x < Width; x++)
                {
                    if (y >= (Height - filledLines) && x != blank && random.Next(0, 10) < fillProb)
                    {
                        Blocks[x, y] = new Block
                        {
                            X = x,
                            Y = y,
                            Color = random.Next(0, NColors)
                        };
                    }
                    else
                        Blocks[x, y] = null;
                }
            }

            if (!pickDifficultBlocks)
                NextShape = PickRandomShape();

            Ready = true;
            GameOver = false;
        }

        public Game Copy()
        {
            var game = new Game();
            if (Shape != null)
                game.Shape = Shape.Copy();
            if (NextShape != null)
                game.NextShape = NextShape.Copy();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (Blocks[x, y] != null)
                        game.Blocks[x, y] = Blocks[x, y].Copy();
                }
            }

            game.NLinesDestroyed = NLinesDestroyed;
            game.Score = Score;
            game._startingLevel = _startingLevel;
            game._pickDifficultBlocks = _pickDifficultBlocks;
            game._fastMoveDirection = _fastMoveDirection;
            game._fastForward = _fastForward;
            game._hasStarted = _hasStarted;
            game._paused = _paused;
            game.GameOver = GameOver;

            return game;
        }

        public void Start()
        {
            Ready = false;
            _hasStarted = true;
            AddShape();
            SetupDropTimer();
            Started();
            PauseChanged();
        }

        public bool MoveLeft()
        {
            return MoveDirection(-1);
        }

        public bool MoveRight()
        {
            return MoveDirection(1);
        }

        public bool StopMoving()
        {
            if (GameOver)
                return false;

            if (_fastMoveTimeout != null)
                EventTimerManager.Remove(_fastMoveTimeout);
            _fastMoveTimeout = null;
            _fastMoveDirection = 0;

            return true;
        }

        public bool RotateLeft()
        {
            return MoveShape(0, 0, 1);
        }

        public bool RotateRight()
        {
            return MoveShape(0, 0, -1);
        }

        public void SetFastForward(bool enable)
        {
            if (_fastForward == enable || GameOver)
                return;
            if (enable)
                if (!MoveShape(0, 1, 0))
                    return;
            _fastForward = enable;
            SetupDropTimer();
        }

        public void Drop()
        {
            if (Shape == null)
                return;

            while (MoveShape(0, 1, 0)) ;
            FallTimeoutCb();
        }

        public void Stop()
        {
            if (_dropTimeout != null)
                EventTimerManager.Remove(_dropTimeout);
        }

        private bool MoveDirection(int direction)
        {
            if (GameOver)
                return false;
            if (_fastMoveDirection == direction)
                return true;

            if (_fastMoveTimeout != null)
                EventTimerManager.Remove(_fastMoveTimeout);
            _fastMoveTimeout = null;
            _fastMoveDirection = direction;
            if (!Move())
                return false;

            _fastMoveTimeout = EventTimerManager.Add(500, SetupFastMoveCb);

            return true;
        }

        private bool SetupFastMoveCb()
        {
            if (!Move())
                return false;
            _fastMoveTimeout = EventTimerManager.Add(40, Move);

            return false;
        }

        private bool Move()
        {
            if (!MoveShape(_fastMoveDirection, 0, 0))
            {
                _fastMoveTimeout = null;
                _fastMoveDirection = 0;

                return false;
            }

            return true;
        }

        private void SetupDropTimer()
        {
            var timestep = (int)Math.Round(80 + 800.0 * Math.Pow(0.75, Level - 1));
            timestep = Math.Max(10, timestep);

            /* In fast forward mode drop at the fastest rate */
            if (_fastForward)
                timestep = 80;

            if (_dropTimeout != null)
                EventTimerManager.Remove(_dropTimeout);
            _dropTimeout = null;

            if (!Paused)
                _dropTimeout = EventTimerManager.Add(timestep, FallTimeoutCb);
        }

        private bool FallTimeoutCb()
        {
            /* Drop the shape down, and create a new one when it can't move */
            if (!MoveShape(0, 1, 0))
            {
                /* Destroy any lines created */
                LandShape();

                /* Add a new shape */
                AddShape();
            }

            return true;
        }

        private void AddShape()
        {
            if (_pickDifficultBlocks)
            {
                var difficultShapes = PickDifficultShapes();
                Shape = difficultShapes[0];
                NextShape = difficultShapes[1];
            }
            else
            {
                Shape = NextShape;
                NextShape = PickRandomShape();
            }

            foreach (var block in Shape.Blocks)
            {
                var x = Shape.X + block.X;
                var y = Shape.Y + block.Y;

                /* Abort if can't place there */
                if (y >= 0 && Blocks[x, y] != null)
                {
                    // FIXME: Place it where it can fit

                    if (_dropTimeout != null)
                        EventTimerManager.Remove(_dropTimeout);
                    _dropTimeout = null;
                    Shape = null;
                    GameOver = true;
                    Complete();
                    return;
                }
            }

            ShapeAdded();
        }

        private Shape PickRandomShape()
        {
            var random = new Random();
            return MakeShape(random.Next(0, NColors), random.Next(0, 4));
        }

        private Shape[] PickDifficultShapes()
        {
            /* The algorithm comes from Federico Poloni's "bastet" game */
            var metrics = new int[NColors];
            for (var type = 0; type < NColors; type++)
            {
                metrics[type] = -32000;
                for (var rotation = 0; rotation < 4; rotation++)
                {
                    for (var pos = 0; pos < Width; pos++)
                    {
                        /* Copy the current game and create a block of the given type */
                        var g = Copy();
                        g._pickDifficultBlocks = false;
                        g.Shape = MakeShape(type, rotation);

                        /* Move tile to position from the left */
                        var validPosition = true;
                        while (g.MoveShape(-1, 0, 0)) ;
                        for (var x = 0; x < pos; x++)
                        {
                            if (!g.MoveShape(1, 0, 0))
                            {
                                validPosition = false;
                                break;
                            }
                        }

                        if (!validPosition)
                            break;

                        /* Drop the tile here and check the metric */
                        var origLines = g.NLinesDestroyed;
                        g.Drop();

                        /* High metric for each line destroyed */
                        var metric = (g.NLinesDestroyed - origLines) * 5000;

                        /* Low metric for large columns */
                        for (var x = 0; x < Width; x++)
                        {
                            int y;
                            for (y = 0; y < Height; y++)
                            {
                                if (g.Blocks[x, y] != null)
                                    break;
                            }

                            metric -= 5 * (Height - y);
                        }

                        if (metric > metrics[type])
                            metrics[type] = metric;

                        /* Destroy this copy */
                        g.Stop();
                    }
                }
            }

            var random = new Random();

            /* Perturb score (-2 to +2), to avoid stupid tie handling */
            for (var i = 0; i < NColors; i++)
                metrics[i] += random.Next(-2, 2);

            /* Sorts possible_types by priorities, worst (interesting to us) first */
            var possibleTypes = new int[NColors];
            for (var i = 0; i < NColors; i++)
                possibleTypes[i] = i;
            for (var i = 0; i < NColors; i++)
            {
                for (var j = 0; j < NColors - 1; j++)
                {
                    if (metrics[possibleTypes[j]] > metrics[possibleTypes[j + 1]])
                    {
                        int t = possibleTypes[j];
                        possibleTypes[j] = possibleTypes[j + 1];
                        possibleTypes[j + 1] = t;
                    }
                }
            }

            /* Actually choose a piece */
            var rnd = random.Next(0, 99);
            if (rnd < 75)
                Shape = MakeShape(possibleTypes[0], random.Next(0, 4));
            else if (rnd < 92)
                Shape = MakeShape(possibleTypes[1], random.Next(0, 4));
            else if (rnd < 98)
                Shape = MakeShape(possibleTypes[2], random.Next(0, 4));
            else
                Shape = MakeShape(possibleTypes[3], random.Next(0, 4));

            /* Look, this one is a great fit. It would be a shame if it wouldn't be given next */
            NextShape = MakeShape(possibleTypes[NColors - 1], random.Next(0, 4));

            var shapes = new Shape[2];
            shapes[0] = Shape;
            shapes[1] = NextShape;
            return shapes;
        }

        private Shape MakeShape(int type, int rotation)
        {
            var shape = new Shape
            {
                Type = type,
                Rotation = rotation
            };

            /* Place this block at top of the field */
            var offset = shape.Type * 64 + shape.Rotation * 16;
            var minWidth = 4;
            var maxWidth = 0;
            var minHeight = 4;
            var maxHeight = 0;
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (blockTable[offset + y * 4 + x] == 0)
                        continue;

                    minWidth = Math.Min(x, minWidth);
                    maxWidth = Math.Max(x + 1, maxWidth);
                    minHeight = Math.Min(y, minHeight);
                    maxHeight = Math.Max(y + 1, maxHeight);

                    var b = new Block
                    {
                        Color = shape.Type,
                        X = x,
                        Y = y
                    };
                    shape.Blocks.Add(b);
                }
            }
            var blockWidth = maxWidth - minWidth;
            shape.X = (Width - blockWidth) / 2 - minWidth;
            shape.Y = -minHeight;

            return shape;
        }

        private void LandShape()
        {
            /* Leave these blocks here */
            foreach (var b in Shape.Blocks)
            {
                b.X += Shape.X;
                b.Y += Shape.Y;
                Blocks[b.X, b.Y] = b;
            }

            var fallDistance = 0;
            var lines = new int[4];
            var nLines = 0;
            var baseLineDestroyed = false;
            for (var y = Height - 1; y >= 0; y--)
            {
                var explode = true;
                for (var x = 0; x < Width; x++)
                {
                    if (Blocks[x, y] == null)
                    {
                        explode = false;
                        break;
                    }
                }

                if (explode)
                {
                    if (y == Height - 1)
                        baseLineDestroyed = true;
                    lines[nLines] = y;
                    nLines++;
                }
            }
            Array.Resize(ref lines, nLines);

            List<Block> lineBlocks = new List<Block>();
            for (var y = Height - 1; y >= 0; y--)
            {
                var explode = true;
                for (var x = 0; x < Width; x++)
                {
                    if (Blocks[x, y] == null)
                    {
                        explode = false;
                        break;
                    }
                }

                if (explode)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        lineBlocks.Add(Blocks[x, y]);
                        Blocks[x, y] = null;
                    }
                    fallDistance++;
                }
                else if (fallDistance > 0)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        var b = Blocks[x, y];
                        if (b != null)
                        {
                            b.Y += fallDistance;
                            Blocks[b.X, b.Y] = b;
                            Blocks[x, y] = null;
                        }
                    }
                }
            }

            var oldLevel = Level;

            /* Score points */
            NLinesDestroyed += nLines;
            switch (nLines)
            {
                case 0:
                    break;
                case 1:
                    Score += 40 * Level;
                    break;
                case 2:
                    Score += 100 * Level;
                    break;
                case 3:
                    Score += 300 * Level;
                    break;
                case 4:
                    Score += 1200 * Level;
                    break;
            }
            /* You get a bonus for getting back to the base */
            if (baseLineDestroyed)
                Score += 10000 * Level;

            /* Increase speed if level has changed */
            if (Level != oldLevel)
                SetupDropTimer();

            ShapeLanded(lines, lineBlocks);
            Shape = null;
        }

        private bool MoveShape(int xStep, int yStep, int rStep)
        {
            if (Shape == null)
                return false;

            /* Check it can fit into the new location */
            RotateShape(rStep);
            var canMove = true;
            foreach (var b in Shape.Blocks)
            {
                var x = Shape.X + xStep + b.X;
                var y = Shape.Y + yStep + b.Y;
                if (y < 0 || x < 0 || x >= Width || y >= Height || Blocks[x, y] != null)
                {
                    canMove = false;
                    break;
                }
            }

            /* Place in the new location or put it back where it was */
            if (canMove)
            {
                Shape.X += xStep;
                Shape.Y += yStep;

                if (xStep != 0)
                    ShapeMoved();
                else if (yStep > 0)
                    ShapeDropped();
                else
                    ShapeRotated();
            }
            else
                RotateShape(-rStep);

            return canMove;
        }

        private void RotateShape(int rStep)
        {
            var r = Shape.Rotation + rStep;
            if (r < 0)
                r += 4;
            if (r >= 4)
                r -= 4;

            if (r == Shape.Rotation)
                return;
            Shape.Rotation = r;

            /* Rearrange current blocks */
            var blocks = Shape.Blocks;
            var offset = Shape.Type * 64 + r * 16;

            blocks.Clear();
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    if (blockTable[offset + y * 4 + x] != 0)
                    {                     
                        blocks.Add(new Block
                        {
                            Color = Shape.Type,
                            X = x,
                            Y = y,
                        });
                    }
                }
            }
        }
    }
}

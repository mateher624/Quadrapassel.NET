using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrapassel
{
    public class Shape : Object
    {
        /* Location of shape */
        public int X;
        public int Y;

        /* Rotation angle */
        public int Rotation;

        /* Piece type */
        public int Type;

        /* Blocks that make up this shape */
        public List<Block> Blocks;

        public Shape Copy()
        {
            var s = new Shape
            {
                X = X,
                Y = Y,
                Rotation = Rotation,
                Type = Type
            };

            foreach (var b in Blocks)
                s.Blocks.Add(b.Copy());

            return s;
        }

        public Shape()
        {
            Blocks = new List<Block>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrapassel
{
    public class Block : Object
    {
        /* Location of block */
        public int X;
        public int Y;

        /* Color of block */
        public int Color;

        public Block Copy()
        {
            var b = new Block
            {
                X = X,
                Y = Y,
                Color = Color
            };
            return b;
        }
    }
}

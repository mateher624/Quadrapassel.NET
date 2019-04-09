using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Quadrapassel
{
    public static class ColorConverter
    {
        public static Color Convert(int value)
        {
            var effectiveValue = value % 7;
            switch (value)
            {
                case 0:
                    return Color.Cyan;
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Magenta;
                case 4:
                    return Color.Red;
                case 5:
                    return Color.Yellow;
                case 6:
                    return Color.White;
                default:
                    return Color.Black;
            }
        }
    }
}

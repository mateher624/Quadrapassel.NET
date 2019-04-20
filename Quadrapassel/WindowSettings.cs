using System;
using SFML.Graphics;

namespace Quadrapassel
{
    public class WindowSettings
    {
        public uint WindowHeight { get; set; } = 800;
        public uint WindowWidth { get; set; } = 600;
        public uint FrameLimit { get; set; } = 60;
        public string WindowName { get; set; } = "";
        public Color BackgroundColor { get; set; } = Color.White;
    }
}

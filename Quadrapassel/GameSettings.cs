namespace Quadrapassel
{
    public class GameSettings
    {
        public int Lines { get; set; } = 20;
        public int Columns { get; set; } = 14;
        public int StartingLevel { get; set; } = 1;
        public int FilledLines { get; set; } = 0;
        public int FillProb { get; set; } = 5;
        public bool PickDifficultBlocks { get; set; }
        
    }
}

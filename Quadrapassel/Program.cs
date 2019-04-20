namespace Quadrapassel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings
            {
                WindowSettings = new WindowSettings
                {
                    FrameLimit = 60,
                    WindowHeight = 880,
                    WindowWidth = 880,
                    WindowName = "Quadrapassel"
                },
                GameSettings = new GameSettings
                {
                    Columns = 14,
                    Lines = 20,
                    StartingLevel = 1,
                    FilledLines = 0,
                    FillProb = 0,
                    PickDifficultBlocks = false
                }
            };

            var myGame = new QuadrapasselScene(settings);
            myGame.Run();
        }
    }
}

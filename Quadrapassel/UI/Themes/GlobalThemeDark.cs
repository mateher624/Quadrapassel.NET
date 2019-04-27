using Quadrapassel.Helpers;
using Quadrapassel.UI.Themes.Abstraction;
using SFML.Graphics;

namespace Quadrapassel.UI.Themes
{
    public class GlobalThemeDark : ITheme
    {
        //public Font Font { get; } = new Font("./Resources/SegoeUI.ttf");
        public Font Font { get; } = new Font(EmbeddedResourceManager.GetEmbeddedResourceBytes("Quadrapassel.Resources.SegoeUI.ttf"));

        public Color FontColor { get; } = new Color(255, 255, 255);
        public Color FontSecondaryColor { get; } = new Color(0, 0, 0);

        public Color PrimaryColor { get; } = new Color(0, 120, 215);
        public Color SecondaryColor { get; } = new Color(81, 81, 81);
        public Color BackgroundColor { get; } = new Color(21, 21, 21);

        public Color PrimaryHoveredColor { get; } = new Color(0, 120, 215);
        public Color SecondaryHoveredColor { get; } = new Color(204, 204, 204);

        public Color DisabledColor { get; } = new Color(81, 81, 81);
        public Color ClickedColor { get; } = new Color(173, 173, 173);

        public Texture BlockTexture { get; } = new Texture(EmbeddedResourceManager.GetEmbeddedResourceBytes("Quadrapassel.Resources.Block.png"));
    }
}

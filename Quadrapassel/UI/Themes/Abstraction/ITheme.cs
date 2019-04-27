using SFML.Graphics;

namespace Quadrapassel.UI.Themes.Abstraction
{
    public interface ITheme
    {
        Font Font { get; }
        Color FontColor { get; }
        Color FontSecondaryColor { get; }

        Color PrimaryColor { get; }
        Color SecondaryColor { get; }
        Color BackgroundColor { get; }

        Color PrimaryHoveredColor { get; }
        Color SecondaryHoveredColor { get; }

        Color DisabledColor { get; }
        Color ClickedColor { get; }

        Texture BlockTexture { get; }
    }
}

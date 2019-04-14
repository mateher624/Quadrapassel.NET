using SFML.Graphics;

namespace Quadrapassel.UI.Abstraction
{
    public interface IUIElement : Drawable
    {

    }

    public abstract class UIElement : IUIElement
    {
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}

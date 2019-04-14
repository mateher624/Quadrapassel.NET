using SFML.Window;

namespace Quadrapassel.UI.Abstraction
{
    interface IClickable
    {
        void Clicked(int x, int y, Mouse.Button button);

        void Unclicked(int x, int y, Mouse.Button button);
    }
}

using SFML.Window;

namespace Quadrapassel.KeyEvents.Abstraction
{
    public interface IKeyPressedEventDispatchStrategy
    {
        void Dispatch(Game gameModel, KeyEventArgs keyEventArgs);
    }
}

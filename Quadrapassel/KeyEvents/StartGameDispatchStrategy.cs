using Quadrapassel.KeyEvents.Abstraction;
using SFML.Window;

namespace Quadrapassel.KeyEvents
{
    public class StartGameDispatchStrategy : IKeyPressedEventDispatchStrategy
    {
        public void Dispatch(Game gameModel, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Code == Keyboard.Key.Enter)
                gameModel.Start();
        }
    }
}

using System.Collections.Generic;
using Quadrapassel.KeyEvents;
using Quadrapassel.KeyEvents.Abstraction;
using SFML.Window;

namespace Quadrapassel
{
    public class KeyEventManager
    {
        private readonly Game _gameModel;
        private ICollection<IKeyPressedEventDispatchStrategy> _dispatchStrategies;

        public KeyEventManager(Game gameModel)
        {
            _gameModel = gameModel;
            _dispatchStrategies = new List<IKeyPressedEventDispatchStrategy>
            {
                new StartGameDispatchStrategy()
            };
        }

        public void Dispatch(KeyEventArgs keyEventArgs)
        {
            foreach (var dispatchStrategy in _dispatchStrategies)
            {
                dispatchStrategy.Dispatch(_gameModel, keyEventArgs);
            }
        }
    }
}

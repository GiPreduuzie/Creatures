using Creatures.Language.Commands.Interfaces;

namespace CellsAutomate.ChildCreatingStrategies
{
    public class PlainChildCreatingStrategy : IChildCreatingStrategy
    {
        private readonly LivegivingPrice _price;

        public PlainChildCreatingStrategy(LivegivingPrice price)
        {
            _price = price;
        }

        public LivegivingPrice CountPrice(ICommand[] childsActions, ICommand[] childsDirections)
        {
            return _price;
        }
    }
}
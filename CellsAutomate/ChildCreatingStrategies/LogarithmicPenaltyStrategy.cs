using System;
using Creatures.Language.Commands.Interfaces;

namespace CellsAutomate.ChildCreatingStrategies
{
    public class LogarithmicPenaltyStrategy : IChildCreatingStrategy
    {
        private readonly int _basicPrice;

        public LogarithmicPenaltyStrategy(int basicPrice)
        {
            _basicPrice = basicPrice;
        }

        public LivegivingPrice CountPrice(ICommand[] childsActions, ICommand[] childsDirections)
        {
            var price = Math.Log(childsActions.Length + childsDirections.Length, 10);
            return new LivegivingPrice(_basicPrice + (int)price);
        }
    }
}
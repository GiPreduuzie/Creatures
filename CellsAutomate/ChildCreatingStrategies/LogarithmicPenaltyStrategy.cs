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

        public LivegivingPrice CountPrice(int childsActionsLength)
        {
            var price = Math.Log(childsActionsLength, 10);
            return new LivegivingPrice(_basicPrice + (int)price);
        }
    }
}
using System;

namespace CellsAutomate.ChildCreatingStrategies
{
    public class LogarithmicPenaltyStrategy : IChildCreatingStrategy
    {
        private readonly int _basicPrice;

        public LogarithmicPenaltyStrategy(int basicPrice)
        {
            _basicPrice = basicPrice;
        }

        public LivegivingPrice CountPrice(int childrenActionsLength)
        {
            var price = Math.Log(childrenActionsLength, 10);
            return new LivegivingPrice(_basicPrice + (int)price);
        }
    }
}
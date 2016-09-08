using Creatures.Language.Commands.Interfaces;

namespace CellsAutomate.ChildCreatingStrategies
{
    public class LinearPenaltyStrategy : IChildCreatingStrategy
    {
        private readonly int _basicPrice;

        public LinearPenaltyStrategy(int basicPrice)
        {
            _basicPrice = basicPrice;
        }

        public LivegivingPrice CountPrice(int childsActionsLength)
        {
            var price = childsActionsLength ;
            var start = 150;
            var result = price - start > 0 ? price - start : 0;
            return new LivegivingPrice(_basicPrice + result);
        }
    }
}
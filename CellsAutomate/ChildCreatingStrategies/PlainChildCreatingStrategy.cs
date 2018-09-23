namespace CellsAutomate.ChildCreatingStrategies
{
    public class PlainChildCreatingStrategy : IChildCreatingStrategy
    {
        private readonly LivegivingPrice _price;

        public PlainChildCreatingStrategy(LivegivingPrice price)
        {
            _price = price;
        }

        public LivegivingPrice CountPrice(int childrenActionsLength)
        {
            return _price;
        }
    }
}
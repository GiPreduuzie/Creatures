namespace CellsAutomate.ChildCreatingStrategies
{
    public interface IChildCreatingStrategy
    {
        LivegivingPrice CountPrice(int childrenActionsLength);
    }
}

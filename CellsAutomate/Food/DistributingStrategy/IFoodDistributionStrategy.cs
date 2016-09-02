namespace CellsAutomate.Food.DistributingStrategy
{
    public interface IFoodDistributionStrategy
    {
        void Build(bool[,] creatures, FoodMatrix eatMatrix);
    }
}

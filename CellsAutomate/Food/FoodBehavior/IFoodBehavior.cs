namespace CellsAutomate.Food.FoodBehavior
{
    public interface IFoodBehavior
    {
        void Manage(bool[,] creatures, int height, int width, int[,] eatMatrix);
    }
}
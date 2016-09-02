using System.Drawing;

namespace CellsAutomate.Food.DistributingStrategy
{
    public class FillingOfEntireFieldStrategy : IFoodDistributionStrategy
    {
        public void Build(bool[,] creatures, FoodMatrix eatMatrix)
        {
            for (var i = 0; i < eatMatrix.Length; i++)
            {
                for (var j = 0; j < eatMatrix.Height; j++)
                {
                    if (!creatures[i, j]
                        && !eatMatrix.HasMaxFoodLevel(new Point(i, j)))
                    {
                        eatMatrix.AddFood(new Point(i, j));
                    }
                }
            }
        }
    }
}

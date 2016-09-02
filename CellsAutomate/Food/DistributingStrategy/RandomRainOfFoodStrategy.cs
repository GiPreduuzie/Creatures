using System;
using System.Drawing;

namespace CellsAutomate.Food.DistributingStrategy
{
    public class RandomRainOfFoodStrategy : IFoodDistributionStrategy
    {
        private readonly int _thikness;
        private readonly Random _random = new Random();

        public RandomRainOfFoodStrategy(double thikness)
        {
            _thikness = (int) (1/thikness);
        }

        public void Build(bool[,] creatures, FoodMatrix eatMatrix)
        {
            for (int i = 0; i < eatMatrix.Length; i++)
            {
                for (int j = 0; j < eatMatrix.Height; j++)
                {
                    if (!creatures[i, j] 
                        && !eatMatrix.HasMaxFoodLevel(new Point(i, j))
                        && _random.Next(100) % _thikness == 0)
                    {
                        eatMatrix.AddFood(new Point(i, j));
                    }
                }
            }
        }
    }
}

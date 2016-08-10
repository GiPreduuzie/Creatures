﻿using System;
using System.Drawing;

namespace CellsAutomate.Food
{
    public class RandomRainOfFoodStrategy : IFoodDistributionStrategy
    {
        private readonly int _frequency;

        public RandomRainOfFoodStrategy(int frequency)
        {
            _frequency = frequency;
        }
        
        public void Build(bool[,] creatures, FoodMatrix eatMatrix)
        {
            var random = new Random();

            for (int i = 0; i < eatMatrix.Length; i++)
            {
                for (int j = 0; j < eatMatrix.Height; j++)
                {
                    if (!creatures[i, j] 
                        && !eatMatrix.HasMaxFoodLevel(new Point(i, j))
                        && random.Next(100) % _frequency == 0)
                    {
                        eatMatrix.AddFood(new Point(i, j));
                    }
                }
            }
        }
    }
}

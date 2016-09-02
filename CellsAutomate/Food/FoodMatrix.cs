using System.Drawing;
using CellsAutomate.Constants;
using CellsAutomate.Food.DistributingStrategy;

namespace CellsAutomate.Food
{
    public class FoodMatrix
    {
        private readonly int[,] _matrix;

        private readonly IFoodDistributionStrategy _strategy;
        private readonly int _foodDistributingFrequency;

        private int _counterOfTurns = 0;

        public int Length => _matrix.GetLength(0);
        public int Height => _matrix.GetLength(1);

        public FoodMatrix(int length, int height, IFoodDistributionStrategy strategy, int foodDistributingFrequency)
        {
            _matrix = new int[length, height];
            _strategy = strategy;
            _foodDistributingFrequency = foodDistributingFrequency;
        }

        public bool HasOneBite(Point currentPoint)
        {
            return GetLevelOfFood(currentPoint) >= CreatureConstants.OneBite;
        }

        public bool HasMaxFoodLevel(Point currentPoint)
        {
            return GetLevelOfFood(currentPoint) >= FoodMatrixConstants.MaxFoodLevel;
        }

        public void AddFood(Point currentPoint)
        {
            _matrix[currentPoint.X, currentPoint.Y] += FoodMatrixConstants.AddedFoodLevel;
        }

        public bool TakeFood(Point currentPoint)
        {
            if (!HasOneBite(currentPoint))
                return false;
            _matrix[currentPoint.X, currentPoint.Y] -= CreatureConstants.OneBite;
            return true;
        }

        private int GetLevelOfFood(Point currentPoint)
        {
            return _matrix[currentPoint.X, currentPoint.Y];
        }

        public void Build(bool[,] creatures)
        {
            if (_counterOfTurns%_foodDistributingFrequency == 0)
            {
                _strategy.Build(creatures, this);
            }

            _counterOfTurns++;
        }
    }
}

using System.Drawing;
using CellsAutomate.Constants;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;

namespace CellsAutomate.Food
{
    public class FoodMatrix
    {
        private readonly int[,] _matrix;

        private readonly IFoodDistributionStrategy _strategy;
        private readonly IFoodBehavior _foodBehavior;
        private readonly int _foodDistributingFrequency;

        private int _counterOfTurns = 0;

        public int Length => _matrix.GetLength(0);
        public int Height => _matrix.GetLength(1);

        public FoodMatrix(
            int length, 
            int height, 
            int foodDistributingFrequency,
            IFoodDistributionStrategy strategy,
            IFoodBehavior foodBehavior)
        {
            _matrix = new int[length, height];
            _strategy = strategy;
            _foodDistributingFrequency = foodDistributingFrequency;
            _foodBehavior = foodBehavior;
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

        public int GetLevelOfFood(Point currentPoint)
        {
            return _matrix[currentPoint.X, currentPoint.Y];
        }

        public int GetMaxLevelOfFood()
        {
            var result = -1;

            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (_matrix[i, j] > result)
                    {
                        result = _matrix[i, j];
                    }
                }
            }

            return result;
        }

        public void Build(bool[,] creatures)
        {
            ManageExistingFood(creatures);
            AddNewFood(creatures);

            _counterOfTurns++;
        }

        private void AddNewFood(bool[,] creatures)
        {
            if (_counterOfTurns%_foodDistributingFrequency != 0) return;

            _strategy.Build(creatures, this);
        }

        private void ManageExistingFood(bool[,] creatures)
        {
            _foodBehavior.Manage(creatures, Height, Length, _matrix);
        }

        internal void InitializeMatrixWithFood()
        {
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    AddFood(new Point(i, j));
                }
            }
        }
    }
}

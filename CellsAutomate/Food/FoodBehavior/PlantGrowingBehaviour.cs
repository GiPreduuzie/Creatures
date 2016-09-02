using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CellsAutomate.Food.FoodBehavior
{
    public class PlantGrowingBehaviour : IFoodBehavior
    {
        public void Manage(bool[,] creatures, int height, int width, int[,] eatMatrix)
        {
            var pointsWithFood = SelectPointWithFood(height, width, eatMatrix);

            foreach (var point in pointsWithFood)
            {
                eatMatrix[point.X, point.Y] += 1;
                Grow(height, width, creatures, eatMatrix, point);
            }
        }

        private void Grow(int height, int width, bool[,] creatures, int[,] eatMatrix, Point point)
        {
            var freePoints = GetPointsAround(point).Where(x => IsFree(height, width, eatMatrix, creatures, x));

            foreach (var freePoint in freePoints)
            {
                eatMatrix[freePoint.X, freePoint.Y] += 1;
            }
        }

        private bool IsFree(int height, int width, int[,] eatMatrix, bool[,] creatures, Point point)
        {
            if (point.X < 0 || point.Y < 0) return false;
            if (width <= point.X || height <= point.Y) return false;

            return eatMatrix[point.X, point.Y] <= 0 && !creatures[point.X, point.Y];
        }

        private IEnumerable<Point> GetPointsAround(Point point)
        {
            return new[]
            {
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1)
            }
                .Select(x =>
                {
                    var p = new Point(point.X, point.Y);
                    p.Offset(x);
                    return p;
                });
        }

        private static List<Point> SelectPointWithFood(int height, int width, int[,] eatMatrix)
        {
            var pointsWithFood = new List<Point>();

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var point = new Point(i, j);
                    if (eatMatrix[i, j] > 0) pointsWithFood.Add(point);
                }
            }
            return pointsWithFood;
        }
    }
}
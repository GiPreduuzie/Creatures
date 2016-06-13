using System;
using System.Drawing;
using System.Linq;

namespace CellsAutomate
{
    public class SimpleCreature
    {
        private int _i;
        private int _j;
        private readonly Random _random;
        private int _store = 1;
        private int _turnsOnPlace;
        public int Generation { get; private set; }

        public Random RandomGenerator { get { return _random; } }

        public SimpleCreature(int i, int j, int generation, Random random)
        {
            _i = i;
            _j = j;
            _random = random;
            Generation = generation;
        }

        public Tuple<bool, ActionEnum> MyTurn(bool[,] eatMatrix)
        {
            if (_store == 0)
                return Tuple.Create(false, ActionEnum.Die);

            _store--;

            if (_turnsOnPlace < 4 && _random.Next(3) != 1)
            {
                _store += 3;
                _turnsOnPlace++;
                return Tuple.Create(false, ActionEnum.Stay);
            }
            else
            {
                var directions2 = ActionEx
                        .GetPoints(_i, _j)
                        .Where(x => IsValid(eatMatrix, x))
                        .Where(x => IsFree(eatMatrix, x))
                        .ToArray();

                if (_store > 3 && _random.Next(2) == 1 && directions2.Length != 0)
                {
                    _store -= 3;
                    _turnsOnPlace++;

                    return Tuple.Create(true, ActionEx.ActionByPoint(new Point(_i, _j), directions2[_random.Next(directions2.Length)]));
                }
                else
                {
                    _turnsOnPlace = 0;
                }
            }


            var directions = ActionEx.GetPoints(_i, _j).Where(x => IsValid(eatMatrix, x)).Where(x => IsFree(eatMatrix, x)).ToArray();

            if (!directions.Any()) return Tuple.Create(false, ActionEnum.Die);

            return Tuple.Create(false, ActionEx.ActionByPoint(new Point(_i, _j), directions[_random.Next(directions.Length)]));
        }

        private bool IsFree(bool[,] eatMatrix, Point point)
        {
            return eatMatrix[point.X, point.Y];
        }

        private bool IsValid(bool[,] matrix, Point point)
        {
            if (point.X < 0 || matrix.GetLength(1) <= point.X) return false;
            if (point.Y < 0 || matrix.GetLength(0) <= point.Y) return false;

            return true;
        }

        public void SetPosition(Point pointByAction)
        {
            _i = pointByAction.X;
            _j = pointByAction.Y;
        }
    }
}

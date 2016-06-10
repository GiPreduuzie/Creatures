using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CellsAutomate
{
    public class ReachMatrixBuilder
    {
        public bool[,] Build(
            bool[,] placeHoldersMatrix,
            int pointX,
            int pointY)
        {
            int n = placeHoldersMatrix.GetLength(0);
            int m = placeHoldersMatrix.GetLength(1);

            return Build(placeHoldersMatrix, new bool[n, m], pointX, pointY);
        }

        public bool[,] Build(
            bool[,] placeHoldersMatrix,
            bool[,] reachingMatrix, 
            int pointX,
            int pointY)
        {
            int n = placeHoldersMatrix.GetLength(0);
            int m = placeHoldersMatrix.GetLength(1);

            var stack = new Stack<Point>();
            stack.Push(new Point(pointX, pointY));

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (
                    current.X >= 0
                    && current.X < m
                    && current.Y >= 0
                    && current.Y < n
                    && !placeHoldersMatrix[current.X, current.Y]
                    && !reachingMatrix[current.X, current.Y])
                {
                    reachingMatrix[current.X, current.Y] = true;

                    foreach (var point in ActionEx.GetPoints(current.X, current.Y))
                    {
                        stack.Push(point);
                    }
                }
            }

            return reachingMatrix;
        }
    }

    public enum ActionEnum
    {
        Die,
        Left,
        Right,
        Up,
        Down,
        Stay
    }

    static class ActionEx
    {
        public static Point PointByAction(ActionEnum actionEnum, Point start)
        {
            switch (actionEnum)
            {
                case ActionEnum.Die:
                    throw new ArgumentException();
                case ActionEnum.Left:
                    return new Point(start.X - 1, start.Y);
                case ActionEnum.Right:
                    return new Point(start.X + 1, start.Y);
                case ActionEnum.Up:
                    return new Point(start.X, start.Y - 1);
                case ActionEnum.Down:
                    return new Point(start.X, start.Y + 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionEnum), actionEnum, null);
            }
        }

        public static ActionEnum ActionByPoint(Point start, Point finish)
        {
            var xOffset = finish.X - start.X;
            var yOffset = finish.Y - start.Y;

            if (xOffset == -1 && yOffset == 0) return ActionEnum.Left;
            if (xOffset == 1 && yOffset == 0) return ActionEnum.Right;
            if (xOffset == 0 && yOffset == 1) return ActionEnum.Down;
            if (xOffset == 0 && yOffset == -1) return ActionEnum.Up;

            throw new ArgumentException();
        }

        public static Point[] GetPoints(int i, int j)
        {
            return new[] { new Point(i + 1, j), new Point(i, j + 1), new Point(i - 1, j), new Point(i, j - 1) };
        }
    }

    public class SimpleCreature
    {
        private int _i;
        private int _j;
        private readonly Random _random;
        private int _store = 1;
        private int _turnsOnPlace;

        public Random RandomGenerator { get { return _random; } }

        public SimpleCreature(int i, int j, Random random)
        {
            _i = i;
            _j = j;
            _random = random;
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

    public class Matrix
    {
        public int N;
        public int M;

        public SimpleCreature[,] Cells { get; set; }

        public int AliveCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        if (Cells[i, j] !=null) count++;
                    }
                }

                return count;
            }
        }

        public bool[,] CanBeReached()
        {
            var reachingMatrix = new bool[N, M];
            var placeHoldersMatrix = new bool[N, M];

            for (int  i = 0;  i < N;  i++)
            {
                for (int j = 0; j < M; j++)
                {
                    placeHoldersMatrix[i, j] = Cells != null;
                }
            }


            var reachMatrixBuilder = new ReachMatrixBuilder();

            

            //Do(reachingMatrix, 0, 0);
            reachMatrixBuilder.Build(placeHoldersMatrix, reachingMatrix, 0, M - 1);
            //Do(reachingMatrix, N - 1, 0);
            //Do(reachingMatrix, N - 1, M - 1);

            return reachingMatrix;
        }

        public void FillStartMatrixRandomly()
        {
            var random = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Cells[i, j] =  random.Next(100)%4 == 0 ? new SimpleCreature(i, j, random) : null;
                }
            }
        }


        public string PrintStartMatrix()
        {
            var result = new bool[N, M];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    result[i, j] = Cells[i, j] != null;
                }
            }

            return PrintMatrix(result);
        }

        private string PrintMatrix(bool[,] matrix)
        {
            var result = new StringBuilder();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    result.Append(matrix[i, j] ? "*" : " ");
                }
                result.AppendLine("|");
            }

            return result.ToString();
        }

       

        public bool[,] MakeTurn()
        {
            var matrix = CanBeReached();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    MakeTurn(Cells[i, j], matrix, i, j);
                }
            }

            return matrix;
        }

        private void MakeTurn(SimpleCreature simpleCreature, bool[,] eat, int i, int j)
        {
            if (simpleCreature == null) return;

            var resultTuple = simpleCreature.MyTurn(eat);

            var result = resultTuple.Item2;

            if (resultTuple.Item1)
            {
                var newPosition = ActionEx.PointByAction(result, new Point(i, j));

                var child = new SimpleCreature(newPosition.X, newPosition.Y, simpleCreature.RandomGenerator);

                Cells[newPosition.X, newPosition.Y] = child;

                return;
            }

            if (result == ActionEnum.Stay) return;
            if (result == ActionEnum.Die) Cells[i, j] = null;
            else
            {
                var newPosition = ActionEx.PointByAction(result, new Point(i, j));
                simpleCreature.SetPosition(newPosition);
                Cells[i, j] = null;
                Cells[newPosition.X, newPosition.Y] = simpleCreature;
            }
        }
    }
}
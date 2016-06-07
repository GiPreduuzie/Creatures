using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CellsAutomate
{
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
        private int _store = 0;
        private int _turnsOnPlace;

        public SimpleCreature(int i, int j, Random random)
        {
            _i = i;
            _j = j;
            _random = random;
        }

        public ActionEnum MyTurn(bool[,] eatMatrix)
        {
            if (_turnsOnPlace < 4 && _random.Next(2) == 1)
            {
                _store++;
                _turnsOnPlace++;
                return ActionEnum.Stay;
            }

            if (_store == 3 && _random.Next(2) == 1)
            {
                _store++;
                _turnsOnPlace++;
                return ActionEnum.Stay;
            }


            var directions = ActionEx.GetPoints(_i, _j).Where(x => IsValid(eatMatrix, x)).Where(x => IsFree(eatMatrix, x)).ToArray();

            if (!directions.Any()) return ActionEnum.Die;

            return ActionEx.ActionByPoint(new Point(_i, _j), directions[_random.Next(directions.Length)]);
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

            Do(reachingMatrix, 0, 0);
            Do(reachingMatrix, 0, M - 1);
            Do(reachingMatrix, N - 1, 0);
            Do(reachingMatrix, N - 1, M - 1);

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

        public string PrintMatrixOfReach()
        {
            var matrix = CanBeReached();

            return PrintMatrix(matrix);
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

        private bool[,] Do(bool[,] reachingMatrix, int pointX, int pointY)
        {
            var stack = new Stack<Point>();
            stack.Push(new Point(pointX, pointY));
            
            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (
                    current.X >= 0
                    && current.X < M 
                    && current.Y >= 0 
                    && current.Y < N 
                    && (Cells[current.X, current.Y] == null) 
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

        public void MakeTurn()
        {
            var matrix = CanBeReached();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    MakeTurn(Cells[i, j], matrix, i, j);
                }
            }
        }

        private void MakeTurn(SimpleCreature simpleCreature, bool[,] eat, int i, int j)
        {
            if (simpleCreature == null) return;

            var result = simpleCreature.MyTurn(eat);

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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

    

    public class Matrix
    {
        public int N;
        public int M;

        public SimpleCreature[,] Cells { get; set; }

        public IEnumerable<SimpleCreature> CellsAsEnumerable
        {
            get
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        if (Cells[i,j] != null)
                            yield return Cells[i, j];
                    }
                }
            }
        }

        public bool[,] Eat { get; private set; }

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
                    placeHoldersMatrix[i, j] = Cells[i, j] != null;
                }
            }


            var reachMatrixBuilder = new ReachMatrixBuilder();

            reachMatrixBuilder.Build(placeHoldersMatrix, reachingMatrix, 0, 0);
            reachMatrixBuilder.Build(placeHoldersMatrix, reachingMatrix, 0, M - 1);
            reachMatrixBuilder.Build(placeHoldersMatrix, reachingMatrix, N - 1, 0);
            reachMatrixBuilder.Build(placeHoldersMatrix, reachingMatrix, N - 1, M - 1);

            return reachingMatrix;
        }

        public void FillStartMatrixRandomly()
        {
            var random = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Cells[i, j] =  random.Next(100)%4 == 0 ? new SimpleCreature(i, j, 1, random) : null;
                }
            }

            Eat = CanBeReached();
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

        public void MakeTurn()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    MakeTurn(Cells[i, j], Eat, i, j);
                }
            }

            Eat = CanBeReached();
        }

        private void MakeTurn(SimpleCreature simpleCreature, bool[,] eat, int i, int j)
        {
            if (simpleCreature == null) return;
            
            var resultTuple = simpleCreature.MyTurn(eat);

            var result = resultTuple.Item2;

            if (resultTuple.Item1)
            {
                var newPosition = ActionEx.PointByAction(result, new Point(i, j));

                var child = new SimpleCreature(newPosition.X, newPosition.Y, simpleCreature.Generation + 1, simpleCreature.RandomGenerator);

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
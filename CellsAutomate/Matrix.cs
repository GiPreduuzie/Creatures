﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CellsAutomate.Food;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using CellsAutomate.Tools;
using CellsAutomate.ChildCreatingStrategies;

namespace CellsAutomate
{
    public class Matrix
    {
        public readonly int Length;
        public readonly int Height;
        public FoodMatrix EatMatrix { get; }
        private readonly Creator _creator;
        private readonly IChildCreatingStrategy _childCreatingStrategy;

        public Membrane[,] Creatures { get; }

        public int GetNationsAmount()
        {
            return CreaturesAsEnumerable.Any() ? CreaturesAsEnumerable.Select(x => x.ParentMark).Distinct().Count() : 0;
        }

        public int GetMaxEnergy()
        {
            return CreaturesAsEnumerable.Any() ? CreaturesAsEnumerable.Select(x => x.EnergyPoints).Max() : 0;
        }

        public int GetMaxAge()
        {
            return CreaturesAsEnumerable.Any() ? CreaturesAsEnumerable.Select(x => x.Age).Max() : 0;
        }

        public Matrix(
            int length, 
            int height,
            Creator creator,
            IFoodDistributionStrategy strategy,
            IFoodBehavior foodBehavior,
            int foodDistributingFrequency,
            IChildCreatingStrategy childCreatingStrategy)
        {
            Length = length;
            Height = height;
            _creator = creator;
            EatMatrix = new FoodMatrix(length, height, foodDistributingFrequency, strategy, foodBehavior);
            Creatures = new Membrane[length, height];
            _childCreatingStrategy = childCreatingStrategy;
        }

        public IEnumerable<Membrane> CreaturesAsEnumerable => Creatures.OfType<Membrane>();

        public int AliveCount => CreaturesAsEnumerable.Count();

        private void FillMatrixWithFood()
        {
            var placeHoldersMatrix = new bool[Length, Height];

            for (var i = 0; i < Length; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    placeHoldersMatrix[i, j] = Creatures[i, j] != null;
                }
            }

            EatMatrix.Build(placeHoldersMatrix);
        }

        public void FillStartMatrixRandomly()
        {
            var random = new Random();

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Creatures[i, j] = random.Next(1000) % 100 == 0 ? new Membrane(_creator.CreateAbstractCreature(SendMessage), random, new Point(i, j), 1, 0, _creator) : null;
                }
            }

            EatMatrix.InitializeMatrixWithFood();
        }

        public void MakeTurn(ExecutionSettings settings)
        {
            var creatures = CreaturesAsEnumerable.ToList();

            if (settings.RandomOrder)
            {
                var random = new Random();
                creatures = creatures.OrderBy(x => random.Next()).ToList();
            }

            if (settings.RunInParallel)
            {
                Parallel.ForEach(creatures, MakeTurn);
            }
            else
            {
                foreach (var item in creatures)
                {
                    MakeTurn(item);
                }
            }

            FillMatrixWithFood();
        }


        private void SendMessage(Point currentPosition, int receiverPosition, int message)
        {
            Point point;
            switch (receiverPosition)
            {
                case 0:
                    point = new Point(currentPosition.X, currentPosition.Y - 1);
                    break;
                case 1:
                    point = new Point(currentPosition.X + 1, currentPosition.Y);
                    break;
                case 2:
                    point = new Point(currentPosition.X, currentPosition.Y + 1);
                    break;
                case 3:
                    point = new Point(currentPosition.X - 1, currentPosition.Y);
                    break;
                default: throw new Exception();
            }

            Creatures[point.X, point.Y]?.Creature.ReceiveMessage(message);
        }

        private void MakeTurn(Membrane currentCreature)
        {
            try
            {
                var turnResult = currentCreature.Turn(EatMatrix, Creatures);
                var action = turnResult.Item1;
                var direction = turnResult.Item2;

                switch (action)
                {
                    case ActionEnum.Die:
                        MakeTurnDie(currentCreature.Position); break;
                    case ActionEnum.MakeChild:
                        MakeTurnMakeChild(direction, currentCreature); break;
                    case ActionEnum.Go:
                        MakeTurnGo(direction, currentCreature); break;
                    case ActionEnum.Eat:
                        var answerQuality = currentCreature.SolveTask();
                        currentCreature.Eat(EatMatrix, answerQuality); break;
                    default: throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MakeTurnDie(currentCreature.Position);
                EXCEPTIONS++;
            }
        }

        public static int EXCEPTIONS = 0;
        private void MakeTurnDie(Point position)
        {
            Creatures[position.X, position.Y] = null;
        }

        private void MakeTurnMakeChild(DirectionEnum direction, Membrane creature)
        {
            if (direction == DirectionEnum.Stay)
                return;
            var childPoint = DirectionEx.PointByDirection(direction, creature.Position);
            if (CommonMethods.IsValidAndFree(childPoint, Creatures) && HasEnoughtEnergyForChild(creature))
            {
                Creatures[childPoint.X, childPoint.Y] = creature.MakeChild(childPoint, SendMessage);
            }
        }

        private bool HasEnoughtEnergyForChild(Membrane creature)
        {
            var energyPoints = creature.EnergyPoints;
            return energyPoints >= _childCreatingStrategy.CountPrice(creature.Creature.GenotypeLength).Price;
        }

        private void MakeTurnGo(DirectionEnum direction, Membrane creature)
        {
            if (direction == DirectionEnum.Stay)
                return;
            var newPosition = DirectionEx.PointByDirection(direction, creature.Position);

            if (!CommonMethods.IsValidAndFree(newPosition, Creatures)) return;
            creature.Move(Creatures, newPosition);
            Stats.AddStats(direction);
        }
    }

    public class ExecutionSettings
    {
        public bool RunInParallel { get; set; }

        public bool RandomOrder { get; set; } = true;
    }
}

using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using Creatures.Language.Commands.Interfaces;
using System;
using System.Globalization;
using CellsAutomate.ChildCreatingStrategies;
using CellsAutomate.Constants;
using CellsAutomate.Mutator;

namespace DependenciesResolver
{
    public class DependencyResolver
    {
        public Matrix GetMatrix()
        {
            return new Matrix(
                GetMatrixSize(),
                GetMatrixSize(),
                GetCreatureCreator(),
                GetFoodDistributionStrategy(),
                GetFoodBehavior(),
                GetFoodDistributingFrequency());
        }

        public string GetPathForLogs()
        {
            return GetString("path for logs");
        }

        private ICommand[] GetDirectionAlgorithm()
        {
            return new DirectionAlgorithmProvider().Algorithm;
        }

        private ICommand[] GetActionAlgorithm()
        {
            return new GetActionAlgorithm().Algorithm;
        }

        public CreatorOfCreature GetCreatureCreator()
        {
            return new CreatorOfCreature(GetMutator(), GetActionAlgorithm(), GetDirectionAlgorithm(), GetChildCreatingStrategy());
        }

        private IChildCreatingStrategy GetChildCreatingStrategy()
        {
            var strategy = GetString("child creation price");
            switch (strategy)
            {
                case "constant": return new PlainChildCreatingStrategy(new LivegivingPrice(CreatureConstants.ChildPrice));
                case "logarithmic penality": return new LogarithmicPenaltyStrategy(CreatureConstants.ChildPrice);

                default: throw new ArgumentException("I know nothing about such strategy: " + strategy);
            }
        }

        private Mutator GetMutator()
        {
            return new Mutator(GetDouble("mutation probability"), new Random());
        }

        private IFoodDistributionStrategy GetFoodDistributionStrategy()
        {
            var strategy = GetString("food distibution strategy");
            switch (strategy)
            {
                case "as water from corners": return new FillingFromCornersByWavesStrategy();
                case "random rain": return new RandomRainOfFoodStrategy(GetDouble("rain thikness"));
                case "fill entire field": return new FillingOfEntireFieldStrategy();

                default: throw new ArgumentException("I know nothing about this strategy: " + strategy);
            }
        }

        private IFoodBehavior GetFoodBehavior()
        {
            var strategy = GetString("food behavior");
            switch (strategy)
            {
                case "plain": return new PlainBehaviour();
                case "grow": return new PlantGrowingBehaviour();

                default: throw new ArgumentException("I know nothing about this strategy: " + strategy);
            }
        }

        private int GetFoodDistributingFrequency()
        {
            return GetInt("food distribution frequency");
        }

        public int GetMatrixSize()
        {
            return GetInt("matrix size");
        }

        private int GetInt(string key)
        {
            return int.Parse(GetString(key));
        }

        private double GetDouble(string key)
        {
            return double.Parse(GetString(key), new CultureInfo("en-US"));
        }

        private string GetString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}

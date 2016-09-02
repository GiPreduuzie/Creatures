using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using Creatures.Language.Commands.Interfaces;
using System;

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
            return new GetDirectionAlgorithm().Algorithm;
        }

        private ICommand[] GetActionAlgorithm()
        {
            return new GetActionAlgorithm().Algorithm;
        }

        public CreatorOfCreature GetCreatureCreator()
        {
            return new CreatorOfCreature(GetActionAlgorithm(), GetDirectionAlgorithm());
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
            return double.Parse(GetString(key), System.Globalization.NumberStyles.Float);
        }

        private string GetString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}

using System;
using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.Food;
using Creatures.Language.Commands.Interfaces;

namespace ConsoleWorker
{
    public class DependencyResolver
    {
        public Matrix GetMatrix()
        {
            return new Matrix(GetMatrixSize(), GetMatrixSize(), GetCreatureCreator(), GetFoodDistributionStrategy());
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
            var frequency = GetInt("food distribution frequency");

            var strategy = GetString("food distibution strategy");
            switch (strategy)
            {
                case "as water from corners": return new FillingFromCornersByWavesStrategy(frequency);
                case "random rain": return new RandomRainOfFoodStrategy(frequency);
                case "fill entire field": return new FillingOfEntireFieldStrategy(frequency);

                default: throw new ArgumentException("I know nothing about this strategy: " + strategy);
            }
        }

        public int GetMatrixSize()
        {
            return GetInt("matrix size");
        }

        private int GetInt(string key)
        {
            return int.Parse(GetString(key));
        }

        private string GetString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}
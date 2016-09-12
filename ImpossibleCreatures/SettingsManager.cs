using System;
using System.Globalization;
using System.Windows.Controls;
using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.ChildCreatingStrategies;
using CellsAutomate.Constants;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using CellsAutomate.Mutator;
using Creatures.Language.Commands.Interfaces;
using ImpossibleCreatures.Settings;

namespace ImpossibleCreatures
{
    class SettingsManager
    {
        private string GetSelectedValue(ComboBox comboBox)
        {
            return ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
        }
        public string GetPathForLogs()
        {
            return GetString("path for logs");
        }

        private ICommand[] GetActionAlgorithm()
        {
            return new ActionExperimentalAlgorithm().Algorithm;
        }

        public CreatorOfCreature GetCreatureCreator()
        {
            return new CreatorOfCreature(GetMutator(), GetActionAlgorithm(), GetChildCreatingStrategy());
        }

        private IChildCreatingStrategy GetChildCreatingStrategy(ComboBox childCreationPrice)
        {
            var type = GetSelectedType<ChildCreationPrice>(GetSelectedValue(childCreationPrice));

            switch (type)
            {
                case ChildCreationPrice.Constant:
                    return new PlainChildCreatingStrategy(new LivegivingPrice(CreatureConstants.ChildPrice));
                case ChildCreationPrice.LogarithmicPenality:
                    return new LogarithmicPenaltyStrategy(CreatureConstants.ChildPrice);
                case ChildCreationPrice.LinearPenality:
                    return new LinearPenaltyStrategy(CreatureConstants.ChildPrice);
                default:
                    return null;
            }
        }

        private T GetSelectedType<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        private Mutator GetMutator()
        {
            return new Mutator(GetDouble("mutation probability"));
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

        private IFoodBehavior GetFoodBehavior(ComboBox childCreationPrice)
        {
            var type = GetSelectedType<FoodBehavior>(GetSelectedValue(childCreationPrice));

            switch (type)
            {
                case FoodBehavior.Plain:
                    return new PlainBehaviour();
                case FoodBehavior.Grow:
                    return new PlantGrowingBehaviour();
                default:
                    return null;
            }
        }

        private int GetFoodDistributingFrequency()
        {
            return GetInt("food distribution frequency");
        }

        public int GetMatrixSize(ComboBox matrixSize)
        {
            return int.Parse(GetSelectedValue(matrixSize));
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

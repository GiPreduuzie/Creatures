using System;
using System.Windows.Controls;
using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.ChildCreatingStrategies;
using CellsAutomate.Constants;
using CellsAutomate.Food.DistributingStrategy;
using CellsAutomate.Food.FoodBehavior;
using CellsAutomate.Mutator;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Commands;

namespace ImpossibleCreatures.Settings
{
    class SettingsManager
    {
        private string GetSelectedValue(ComboBox comboBox)
        {
            return ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
        }

        private int GetSelectedValue(Slider slider)
        {
            return (int)slider.Value;
        }
        private ICommand[] GetActionAlgorithm()
        {
            return new ActionExperimentalAlgorithm().Algorithm;
        }

        public CreatorOfCreature GetCreatureCreator(ComboBox childCreationPrice)
        {
            return new CreatorOfCreature(GetMutator(), GetActionAlgorithm(), new ICommand[] { new Stop()}, GetChildCreatingStrategy(childCreationPrice));
        }

        public int GetFoodDistributingFrequency(Slider slider)
        {
            return GetSelectedValue(slider);
        }

        public IChildCreatingStrategy GetChildCreatingStrategy(ComboBox childCreationPrice)
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
            return new Mutator(0.01);
        }

        public IFoodDistributionStrategy GetFoodDistributionStrategy(ComboBox foodDistibutionStrategy)
        {
            var type = GetSelectedType<FoodDistibutionStrategy>(GetSelectedValue(foodDistibutionStrategy));

            switch (type)
            {
                case FoodDistibutionStrategy.AsWaterFromCorners:
                    return new FillingFromCornersByWavesStrategy();
                case FoodDistibutionStrategy.RandomRain:
                    return new RandomRainOfFoodStrategy(0.1);
                case FoodDistibutionStrategy.FillEntireField:
                    return new FillingOfEntireFieldStrategy();
                default:
                    return null;
            }
        }

        public IFoodBehavior GetFoodBehavior(ComboBox childCreationPrice)
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

        public int GetMatrixSize(ComboBox matrixSize)
        {
            return int.Parse(GetSelectedValue(matrixSize));
        }
    }
}

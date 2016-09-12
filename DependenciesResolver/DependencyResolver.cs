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
                GetFoodDistributingFrequency(),
                GetChildCreatingStrategy());
        }


    }
}

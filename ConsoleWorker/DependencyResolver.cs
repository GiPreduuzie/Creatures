using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.Constants;
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
            return new CreatorOfCreature(GetDirectionAlgorithm(), GetActionAlgorithm());
        }

        public IFoodDistributionStrategy GetFoodDistributionStrategy()
        {
            return new FillingFromCornersByWavesStrategy();
        }

        public int GetMatrixSize()
        {
            return LogConstants.MatrixSize;
        }
    }
}
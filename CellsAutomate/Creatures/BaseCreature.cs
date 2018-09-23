using System;
using System.Drawing;
using CellsAutomate.Food;

namespace CellsAutomate.Creatures
{
    public abstract class BaseCreature
    {
        protected abstract Tuple<ActionEnum, DirectionEnum> GetCreatureDesires(
            FoodMatrix eatMatrix,
            Membrane[,] creatures,
            Point position,
            Random random,
            int energyPoints,
            int levelOfFood);

        public Tuple<ActionEnum, DirectionEnum> MyTurn(
            FoodMatrix eatMatrix, 
            Membrane[,] creatures, 
            Point position, 
            Random random, 
            bool hasOneBite, 
            int energyPoints)
        {
            return GetCreatureDesires
                (eatMatrix,
                creatures,
                position,
                random,
                energyPoints,
                eatMatrix.GetLevelOfFood(position));
        }

        public abstract int GenotypeLength { get; }

        public abstract void ReceiveMessage(int message);

        public abstract int SolveTask(Point position, Random random);
    }
}
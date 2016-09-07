using System;
using System.Drawing;
using CellsAutomate.Food;

namespace CellsAutomate.Creatures
{
    public abstract class BaseCreature
    {
        protected abstract DirectionEnum GetDirection(FoodMatrix eatMatrix, Membrane[,] creatures, Point position, Random random);

        protected abstract ActionEnum GetAction(Random random, bool hasOneBite, int energyPoints, int foodOnCell);

        public Tuple<ActionEnum, DirectionEnum> MyTurn(FoodMatrix eatMatrix, Membrane[,] creatures, Point position, 
            Random random, bool hasOneBite, int energyPoints)
        {
            var action = GetAction(random, hasOneBite, energyPoints, eatMatrix.GetLevelOfFood(position));
            var direction = (action == ActionEnum.Eat || action == ActionEnum.MakeChild) 
                ? DirectionEnum.Stay : GetDirection(eatMatrix, creatures, position, random);
            return Tuple.Create(action, direction);
        }

        public abstract int GenotypeLength { get; }
    }
}

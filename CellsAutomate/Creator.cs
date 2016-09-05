using System;
using System.Linq;
using System.Text;
using CellsAutomate.Creatures;
using CellsAutomate.Mutator.CommandsList;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Executors;

namespace CellsAutomate
{
    public abstract class Creator
    {
        public abstract BaseCreature CreateAbstractCreature();
        public abstract BaseCreature MakeChild(BaseCreature parent);
    }

    public class CreatorOfCreature : Creator
    {
        private readonly Mutator.Mutator _mutator;
        private readonly ICommand[] _commandsForGetDirection;
        private readonly ICommand[] _commandsForGetAction;

        public CreatorOfCreature(Mutator.Mutator mutator, ICommand[] commandsForGetAction, ICommand[] commandsForGetDirection)
        {
            _mutator = mutator;
            _commandsForGetAction = commandsForGetAction;
            _commandsForGetDirection = commandsForGetDirection;
        }

        public override BaseCreature CreateAbstractCreature()
        {
            var executor = new Executor();
            return new Creature(executor, _commandsForGetDirection, _commandsForGetAction);
        }

        public override BaseCreature MakeChild(BaseCreature parent)
        {
            var parentAsCreature = parent as Creature;
            if (parentAsCreature == null) throw new ArgumentException();

            var childsDirections = Mutate(parentAsCreature.CommandsForGetDirection);
            var childsActions = Mutate(parentAsCreature.CommandsForGetAction);
            var executor = new Executor();
            var child = new Creature(executor, childsDirections, childsActions);
            return child;
        }

        private ICommand[] Mutate(ICommand[] commands)
        {
            var commandsList = new CommandsList(commands);
            _mutator.Mutate(commandsList);
            return commandsList.ToArray();
        }
    }

    public class CreatorOfSimpleCreature : Creator
    {
        public override BaseCreature CreateAbstractCreature()
        {
            return new SimpleCreature();
        }

        public override BaseCreature MakeChild(BaseCreature parent)
        {
            if(!(parent is SimpleCreature))throw new ArgumentException();
            return new SimpleCreature();
        }
    }


}

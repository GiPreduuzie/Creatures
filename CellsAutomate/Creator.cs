using System;
using System.Drawing;
using System.Linq;
using System.Text;
using CellsAutomate.ChildCreatingStrategies;
using CellsAutomate.Constants;
using CellsAutomate.Creatures;
using CellsAutomate.Mutator.CommandsList;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Executors;

namespace CellsAutomate
{
    public abstract class Creator
    {
        public abstract BaseCreature CreateAbstractCreature(Action<Point, int, int> sendMessage);
        public abstract Tuple<BaseCreature, LivegivingPrice> MakeChild(BaseCreature parent, Action<Point, int, int> sendMessage);
    }

    public class CreatorOfCreature : Creator
    {
        private readonly Mutator.Mutator _mutator;
        private readonly ICommand[] _commandsForGetAction;
        private readonly ICommand[] _commandsForSolution;
        private readonly IChildCreatingStrategy _childCreatingStrategy;

        public CreatorOfCreature(
            Mutator.Mutator mutator,
            ICommand[] commandsForGetAction,
            ICommand[] commandsForSolution,
            IChildCreatingStrategy childCreatingStrategy)
        {
            _mutator = mutator;
            _commandsForGetAction = commandsForGetAction;
            _commandsForSolution = commandsForSolution;
            _childCreatingStrategy = childCreatingStrategy;
        }

        public override BaseCreature CreateAbstractCreature(Action<Point, int, int> sendMessage)
        {
            var executor = new Executor();
            return new Creature(executor, _commandsForGetAction, _commandsForSolution, sendMessage);
        }

        public override Tuple<BaseCreature, LivegivingPrice> MakeChild(BaseCreature parent, Action<Point, int, int> sendMessage)
        {
            var parentAsCreature = parent as Creature;
            if (parentAsCreature == null) throw new ArgumentException();

            var childsActions = Mutate(parentAsCreature.CommandsForGetAction);
            var commandsForSolution = Mutate(parentAsCreature.CommandsForSolution);

            var executor = new Executor();
            BaseCreature child = new Creature(executor, childsActions, commandsForSolution, sendMessage);
            return Tuple.Create(child, _childCreatingStrategy.CountPrice(childsActions.Length));
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
        //public override BaseCreature CreateAbstractCreature()
        //{
        //    return new SimpleCreature();
        //}

        //public override Tuple<BaseCreature, LivegivingPrice> MakeChild(BaseCreature parent)
        //{
        //    if(!(parent is SimpleCreature))throw new ArgumentException();
        //    BaseCreature simpleCreature = new SimpleCreature();
        //    return Tuple.Create(simpleCreature, new LivegivingPrice(CreatureConstants.ChildPrice));
        //}

        public override BaseCreature CreateAbstractCreature(Action<Point, int, int> sendMessage)
        {
            throw new NotImplementedException();
        }

        public override Tuple<BaseCreature, LivegivingPrice> MakeChild(BaseCreature parent, Action<Point, int, int> sendMessage)
        {
            throw new NotImplementedException();
        }
    }
}

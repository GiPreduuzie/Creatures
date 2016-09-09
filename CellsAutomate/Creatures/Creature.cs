using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using CellsAutomate.Food;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Executors;
using CellsAutomate.Tools;
using System.Linq;

namespace CellsAutomate.Creatures
{
    public class Creature : BaseCreature
    {
        private Action<Point, int, int> _sendMessage;
        private readonly Executor _executor;
        private readonly Dictionary<int, int> _creatureMemory = new Dictionary<int, int>();
        private Queue<int> _receivedMessages = new Queue<int>();

        public ICommand[] CommandsForGetAction { get; }

        public Creature(
            Executor executor, 
            ICommand[] commandsForGetAction, Action<Point, int, int> sendMessage)
        {
            _executor = executor;
            CommandsForGetAction = commandsForGetAction;
            _sendMessage = sendMessage;
        }

        protected override Tuple<ActionEnum, DirectionEnum> GetCreatureDesires(
            FoodMatrix eatMatrix,
            Membrane[,] creatures,
            Point position,
            Random random,
            int energyPoints,
            int foodOnCell)
        {
            var environmentState = GetState(eatMatrix, creatures, position);
            environmentState.Add(4, energyPoints);
            environmentState.Add(5, foodOnCell);

            var result = _executor.Execute(CommandsForGetAction, new MyExecutorToolset(random, environmentState, _creatureMemory, (x, y) => _sendMessage(position, x, y), _receivedMessages));
            var results = result
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x.Trim())).ToArray();

            return Tuple.Create(
                ActionEx.ActionByNumber(results[0]),
                DirectionEx.DirectionByNumber(results[1]));
        }

        private Dictionary<int, int> GetState(FoodMatrix eatMatrix, Membrane[,] creatures, Point position)
        {
            var points = CommonMethods.GetPoints(position);
            var state = new Dictionary<int, int>();

            foreach (var point in points)
            {
                var direction = DirectionEx.DirectionByPointsWithNumber(position, point);

                if (CommonMethods.IsValidAndFree(point, creatures))
                    state.Add(direction, eatMatrix.HasOneBite(point) ? 4 : 3);

                if (!CommonMethods.IsValid(point, eatMatrix.Length, eatMatrix.Height))
                    state.Add(direction, 1);
                else
                    if (!CommonMethods.IsFree(point, creatures))
                    state.Add(direction, 2);
            }

            return state;
        }

        public override int GenotypeLength => CommandsForGetAction.Length;

        public override void ReceiveMessage(int message)
        {
            if (_receivedMessages.Count > 100)
                _receivedMessages.Dequeue();

            _receivedMessages.Enqueue(message);
        }
    }
}

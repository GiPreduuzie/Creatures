using System;
using System.Collections.Generic;
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
        public Dictionary<int, int> CreatureMemory { get; } = new Dictionary<int, int>();
        public Queue<int> ReceivedMessages { get; } = new Queue<int>();

        public ICommand[] CommandsForGetAction { get; }
        public ICommand[] CommandsForSolution { get; }


        public Creature(
            Executor executor,
            ICommand[] commandsForGetAction, ICommand[] commandsForSolution, Action<Point, int, int> sendMessage)
        {
            _executor = executor;
            CommandsForGetAction = commandsForGetAction;
            CommandsForSolution = commandsForSolution;
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

            var result = _executor.Execute(CommandsForGetAction, new MyExecutorToolset(random, environmentState, CreatureMemory, (x, y) => _sendMessage(position, x, y), ReceivedMessages));
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
                    state.Add(direction, eatMatrix.HasOneBite(point, Constants.CreatureConstants.OneBite) ? 4 : 3);

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
            if (ReceivedMessages.Count > 100)
                ReceivedMessages.Dequeue();

            ReceivedMessages.Enqueue(message);
        }

        public override int SolveTask(Point position, Random random)
        {
            var sourceData = new Dictionary<int, int>();
            sourceData[0] = 2;
            sourceData[1] = random.Next(100);
            sourceData[2] = random.Next(100);

            var answer = _executor.Execute(CommandsForGetAction, new MyExecutorToolset(random, sourceData, CreatureMemory, (x, y) => _sendMessage(position, x, y), ReceivedMessages));
            var result = answer
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x.Trim())).ToArray();

            if (result.Length == 1)
            {
                if (result[0] == sourceData[1] + sourceData[2])
                    return 200;
                else
                    return 150;
            }
            else
            {
                return 100;
            }
        }
    }
}

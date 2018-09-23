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
        private readonly Action<Point, int, int> _sendMessage;
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

            var result = _executor.Execute(
                CommandsForGetAction,
                CreateExecutorToolset(position, random, environmentState));
            var results = result
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x.Trim())).ToArray();

            return Tuple.Create(
                ActionEx.ActionByNumber(results[0]),
                DirectionEx.DirectionByNumber(results[1]));
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
            var sourceData = new Dictionary<int, int>
            {
                [0] = 2, // arguments amount
                [1] = random.Next(100), // 1st arg
                [2] = random.Next(100)  // 2nd arg
            };

            var answer = _executor.Execute(
                CommandsForSolution,
                CreateExecutorToolset(position, random, sourceData));
            var result = answer
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x.Trim())).ToArray();

            // check if the creature has summed 1st and 2nd
            return result.Length == 1 ? result[0] == sourceData[1] + sourceData[2] ? 200 : 150 : 100;
        }

        private MyExecutorToolset CreateExecutorToolset(
            Point position,
            Random random,
            IDictionary<int, int> state)
        {
            return new MyExecutorToolset(
                random,
                state,
                CreatureMemory,
                (x, y) => _sendMessage(position, x, y),
                ReceivedMessages);
        }

        private static Dictionary<int, int> GetState(FoodMatrix eatMatrix, Membrane[,] creatures, Point position) =>
            CommonMethods.GetPoints(position)
                .Select(point => Tuple.Create(point,
                    CommonMethods.IsValid(point, eatMatrix.Length, eatMatrix.Height)
                        ? CommonMethods.IsFree(point, creatures)
                            ? eatMatrix.HasOneBite(point, Constants.CreatureConstants.OneBite)
                                ? PointStates.HasFood
                                : PointStates.Empty
                            : PointStates.Occupied
                        : PointStates.Invalid))
                .ToDictionary(
                    p => DirectionEx.DirectionByPointsWithNumber(position, p.Item1),
                    p => (int)p.Item2);
    }
}

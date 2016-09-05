using System;
using System.Collections.Generic;
using System.Threading;
using CellsAutomate.Mutator.Mutations;
using Creatures.Language.Executors;

namespace CellsAutomate.Mutator
{
    public class Mutator
    {
        [ThreadStatic]
        private static Random _random;
        public static Random Random => _random ?? (_random = new Random());

        private const int NumOfAttempts = 5;
        private readonly double _mutationProbability;

        private readonly List<Func<CommandsList.CommandsList, IMutation>> _mutations;

        public Mutator(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
            
            _mutations = new List<Func<CommandsList.CommandsList, IMutation>>
            {
                 x => new AddCommandMutation(Random, x),
                 x => new DeleteCommandMutation(Random, x),
                 x => new DublicateCommandMutation(Random, x),
                 x => new ReplaceCommandMutation(Random, x),
                 x => new SwapCommandMutation(Random, x)
            };
        }

        public void Mutate(CommandsList.CommandsList commandsList)
        {
            var mutationCount = GetNumberOfMutations(commandsList);
            for (int i = 0; i < mutationCount; i++)
            {
                MutateSingle(commandsList);
            }
        }

        public static int MUTATIONS = 0;
        public static int MUTATIONSREAL = 0;


        private void MutateSingle(CommandsList.CommandsList commandsList)
        {
            Interlocked.Increment(ref MUTATIONS);
            
            var mutation = GetRandomMutation().Invoke(commandsList);
            for (int i = 0; i < NumOfAttempts; i++)
            {
                Interlocked.Increment(ref MUTATIONSREAL);
                mutation.Transform();
                if (AssertValid(commandsList)) break;
                mutation.Undo();
                Interlocked.Decrement(ref MUTATIONSREAL);
            }
        }

        private bool AssertValid(CommandsList.CommandsList commandsList)
        {
            var executor = new ValidationExecutor();
            return executor.Execute(commandsList, new ExecutorToolset(Random));
        }

        private int GetNumberOfMutations(CommandsList.CommandsList commandsList)
        {
            var mutationCount = 0;
            for (int i = 0; i < commandsList.Count; i++)
            {
                if (ShouldMutate()) mutationCount++;
            }
            return mutationCount;
        }

        private bool ShouldMutate()
        {
            return Random.NextDouble() < _mutationProbability;
        }

        private Func<CommandsList.CommandsList, IMutation> GetRandomMutation()
        {
            return ChooseRandom(_mutations);
        }

        private T ChooseRandom<T>(IReadOnlyList<T> array)
        {
            return array[Random.Next(array.Count)];
        }
    }
}
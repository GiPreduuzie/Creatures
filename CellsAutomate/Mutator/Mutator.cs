using System;
using System.Collections.Generic;
using CellsAutomate.Mutator.Mutations;
using Creatures.Language.Executors;

namespace CellsAutomate.Mutator
{
    public class Mutator
    {
        private readonly Random _random;
        private const int NumOfAttempts = 5;
        private readonly double _mutationProbability;

        private readonly List<Func<CommandsList.CommandsList, IMutation>> _mutations;

        public Mutator(double mutationProbability, Random random)
        {
            _mutationProbability = mutationProbability;
            _random = random;
            _mutations = new List<Func<CommandsList.CommandsList, IMutation>>
            {
                 x => new AddCommandMutation(_random, x),
                 x => new DeleteCommandMutation(_random, x),
                 x => new DublicateCommandMutation(_random, x),
                 x => new ReplaceCommandMutation(_random, x),
                 x => new SwapCommandMutation(_random, x)
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
            MUTATIONS++;
            var mutation = GetRandomMutation().Invoke(commandsList);
            for (int i = 0; i < NumOfAttempts; i++)
            {
                MUTATIONSREAL++;
                mutation.Transform();
                if (AssertValid(commandsList)) break;
                mutation.Undo();
                MUTATIONSREAL--;
            }
        }

        private bool AssertValid(CommandsList.CommandsList commandsList)
        {
            var executor = new ValidationExecutor();
            return executor.Execute(commandsList, new ExecutorToolset(new Random()));
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
            return _random.NextDouble() < _mutationProbability;
        }

        private Func<CommandsList.CommandsList, IMutation> GetRandomMutation()
        {
            return ChooseRandom(_mutations);
        }

        private T ChooseRandom<T>(IReadOnlyList<T> array)
        {
            return array[_random.Next(array.Count)];
        }
    }
}
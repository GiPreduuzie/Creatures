using System;
using System.Collections.Generic;
using System.Linq;
using Creatures.Language.Commands.Interfaces;
using Creatures.ResultMonad;

namespace Creatures.Language.Parsers
{
    public class Parser
    {
        public IEnumerable<ICommand> ProcessCommands(string commands)
        {
            return commands
                .Split(new[] { '\r', '\n' })
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrEmpty(item))
                .Select(ProcessCommand);
        }

        public ICommand ProcessCommand(string command)
        {
            var constructor = new Constructor();

            var handlers = new List<Func<string, IResult<ICommand>>>
            { 
                constructor.NewInt,
                constructor.NewArray,
                constructor.CloneValue,
                constructor.SetValue,
                constructor.Plus,
                constructor.Minus,
                constructor.Print,
                constructor.Condition,
                constructor.CloseCondition,
                constructor.GetState,
                constructor.GetRandom,
                constructor.Stop
            };


            var result = handlers.Select(x => x(command)).FirstOrDefault(x => x.IsSucceded);
            if (result == null) throw new ArgumentException("No proper handler found for : " + command);

            return result.Value;
        }
    }
}
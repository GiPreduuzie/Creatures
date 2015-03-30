using System.Collections.Generic;
using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    class Condition : ICommand
    {
        int _condition;
        IEnumerable<ICommand> _commands;

        public Condition(int condition, IEnumerable<ICommand> commands)
        {
            _condition = condition;
            _commands = commands;
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Collections.Generic;
using System.Text;
using Creatures.Language.Commands;

namespace Creatures
{
    class Executor : ICommandVisitor
    {
        readonly Dictionary<string, int?> _variables = new Dictionary<string, int?>();
        readonly StringBuilder _console = new StringBuilder();

        public string Execute(IEnumerable<ICommand> parsedCommands)
        {
            foreach (var parsedCommand in parsedCommands)
            {
                Execute(parsedCommand);
            }
            return _console.ToString();
        }

        private void Execute(ICommand command)
        {
            command.AcceptVisitor(this);
        }

        public void Accept(NewInt command)
        {
            _variables[command.Name] = null;
        }

        public void Accept(SetValue command)
        {
            _variables[command.Name] = command.Value;
        }

        public void Accept(Plus command)
        {
            var firstValue = _variables[command.FirstSource];
            var secondValue = _variables[command.SecondSource];
            _variables[command.NameTarget] = firstValue + secondValue;
        }

        public void Accept(Print command)
        {
            var value = _variables[command.Variable];
            _console.AppendLine(value.ToString());
        }
    }
}
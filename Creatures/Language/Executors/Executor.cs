using System.Collections.Generic;
using System.Text;
using Creatures.Language.Commands;
using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Executors
{
    public class Executor : ICommandVisitor
    {
        readonly Dictionary<string, int?> _variables = new Dictionary<string, int?>();
        readonly StringBuilder _console = new StringBuilder();
        private Stack<bool> _conditions = new Stack<bool>();

        public string Execute(IEnumerable<ICommand> parsedCommands)
        {
            _conditions.Push(true);
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
            if (!_conditions.Peek()) return;
             
            _variables[command.Name] = null;
        }

        public void Accept(SetValue command)
        {
            if (!_conditions.Peek()) return;
             
            _variables[command.Name] = command.Value;
        }

        public void Accept(Plus command)
        {
            if (!_conditions.Peek()) return;

            var firstValue = _variables[command.FirstSource];
            var secondValue = _variables[command.SecondSource];
            _variables[command.NameTarget] = firstValue + secondValue;
        }

        public void Accept(Minus command)
        {
            if (!_conditions.Peek()) return;

            var firstValue = _variables[command.FirstSource];
            var secondValue = _variables[command.SecondSource];
            _variables[command.NameTarget] = firstValue - secondValue;
        }

        public void Accept(CloneValue command)
        {
            if (!_conditions.Peek()) return;

            var value = _variables[command.SourceName];
            _variables[command.TargetName] = value;
        }

        public void Accept(Condition command)
        {
            if (!_conditions.Peek()) return;

            var value = _variables[command.ConditionName];
            _conditions.Push(value > 0);
        }

        public void Accept(Print command)
        {
            if (!_conditions.Peek()) return;

            var value = _variables[command.Variable];
            _console.AppendLine(value.ToString());
        }

        public void Accept(CloseCondition command)
        {
            _conditions.Pop();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Creatures.Language.Commands;
using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Parsers
{
    public class CommandPrinter : ICommandVisitor
    {
        private readonly StringBuilder _builder;
        private int _embeddedCondtions;

        public CommandPrinter()
        {
            _builder = new StringBuilder();
        }

        public string ParseCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                command.AcceptVisitor(this);
            }
            return GetResult();
        }

        public string ParseCommand(ICommand command)
        {
            command.AcceptVisitor(this);
            return GetResult();
        }

        private string GetResult()
        {
            var res = _builder.ToString().Trim('\r', '\n', ' ');
            _builder.Clear();
            return res;
        }

        public void Accept(NewInt command)
        {
            AddToResult($"int {command.Name}");
        }

        public void Accept(SetValue command)
        {
            AddToResult($"{command.TargetName} = {command.Value}");
        }

        public void Accept(Plus command)
        {
            AddToResult($"{command.TargetName} = {command.FirstSource} + {command.SecondSource}");
        }

        public void Accept(Print command)
        {
            AddToResult($"print {command.Variable}");
        }

        public void Accept(Minus command)
        {
            AddToResult($"{command.TargetName} = {command.FirstSource} - {command.SecondSource}");
        }

        public void Accept(CloneValue command)
        {
            AddToResult($"{command.TargetName} = {command.SourceName}");
        }

        public void Accept(Condition command)
        {
            AddToResult($"if {command.ConditionName} then");
            _embeddedCondtions++;
        }

        public void Accept(Stop command)
        {
            AddToResult("stop");
        }

        public void Accept(CloseCondition command)
        {
            _embeddedCondtions--;
            AddToResult("endif");
        }

        public void Accept(GetState command)
        {
            AddToResult($"{command.TargetName} = getState {command.Direction}");
        }

        public void Accept(GetRandom command)
        {
            AddToResult($"{command.TargetName} = random {command.MaxValueName}");
        }

        public void Accept(SetToMemory command)
        {
            AddToResult($"setToMemory {command.KeyName} {command.ValueName}");
        }

        public void Accept(GetFromMemory command)
        {
            AddToResult($"{command.TargetName} = getFromMemory {command.KeyName}");
        }

        private void AddToResult(string value)
        {
            _builder.AppendLine("".PadLeft(_embeddedCondtions*4, ' ') + value);
        }
    }
}
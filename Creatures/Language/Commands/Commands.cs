using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Language.Commands
{
    interface ICommand
    {

    }

    class NewInt : ICommand
    {
        string _name;

        public NewInt(string name)
        {
            _name = name;
        }
    }

    class NewArray : ICommand
    {
        string _name;
        IEnumerable<int> _value;

        public NewArray(string name, IEnumerable<int> value)
        {
            _name = name;
            _value = value;
        }
    }

    class CloneValue : ICommand
    {
        string _nameTarget;
        string _nameSource;

        public CloneValue(string nameTarget, string nameSource)
        {
            _nameTarget = nameTarget;
            _nameSource = nameSource;
        }
    }

    class SetValue : ICommand
    {
        string _nameTarget;
        int _value;

        public SetValue(string nameTarget, int value)
        {
            _nameTarget = nameTarget;
            _value = value;
        }
    }

    class Plus : ICommand
    {
        string _nameTarget;
        string _firstSource;
        string _secondSource;

        public Plus(string nameTarget, string firstSource, string secondSource)
        {
            _nameTarget = nameTarget;
            _firstSource = firstSource;
            _secondSource = secondSource;
        }
    }

    class Minus : ICommand
    {
        string _nameTarget;
        string _firstSource;
        string _secondSource;

        public Minus(string nameTarget, string firstSource, string secondSource)
        {
            _nameTarget = nameTarget;
            _firstSource = firstSource;
            _secondSource = secondSource;
        }
    }

    class Condition : ICommand
    {
        int _condition;
        IEnumerable<ICommand> _commands;

        public Condition(int condition, IEnumerable<ICommand> commands)
        {
            _condition = condition;
            _commands = commands;
        }
    }
}

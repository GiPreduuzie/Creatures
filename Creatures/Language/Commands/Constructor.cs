using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Language.Commands
{
    class Constructor
    {
        IList<ICommand> commands = new List<ICommand>();

        public ICommand NewInt(string command)
        {
            return new NewInt(CheckTypeNamePair("int", command));
        }

        public IEnumerable<ICommand> ProcessCommands(string commands)
        {
            return commands
                .Split(new[] { '\r', '\n' })
                .Select(item => item.Trim())
                .Where(item => !string.IsNullOrEmpty(item))
                .Select(item => ProcessCommand(item));
        }

        public ICommand ProcessCommand(string command)
        {
            var handlers = new List<Func<string, ICommand>>
            { 
                NewArray,
                CloneValue,
                SetValue,
                Plus,
                Minus,
                Condition
            };

            foreach (var handler in handlers)
            {
                try
                {
                    return handler(command);
                }
                catch
                {
                }
            }

            throw new ArgumentException("No proper handler found for : " + command);
        }

        public ICommand NewArray(string command)
        {
            var exception = new ArgumentException("Should be 'array <name> : v1, v2, v3...', but it is: " + command);

            var parts = command.Split(':').ToList();
            
            if (parts.Count != 2)
                throw exception;

            var name = CheckTypeNamePair("array", parts[0]);

            return new NewArray(name, parts[1].Split(',').Select(item => int.Parse(item.Trim())));
        }

        public ICommand CloneValue(string command)
        {
            var exception = new ArgumentException("Should be '<name_target> = <name_soure>', but it is: " + command);
            var parts = command.Split('=').Select(item=>item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            if (IsIdentifier(parts[0]) && IsIdentifier(parts[1]))
                return new CloneValue(parts[0], parts[1]);

            throw exception;
        }

        public ICommand SetValue(string command)
        {
            var exception = new ArgumentException("Should be '<name_target> = <value>', but it is: " + command);
            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            var value = int.Parse(parts[1]);
            if (IsIdentifier(parts[0]))
                return new SetValue(parts[0], value);

            throw exception;
        }

        public ICommand Plus(string command)
        {
            var exception = new ArgumentException("Should be '<name_target> = <name_source1> + <name_source_2>', but it is: " + command);
            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            var partsLeft = parts[1].Split('+').Select(item => item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            if (IsIdentifier(parts[0]) && IsIdentifier(partsLeft[0]) && IsIdentifier(partsLeft[1]))
                return new Plus(parts[0], partsLeft[0], partsLeft[1]);

            throw exception;
        }

        public ICommand Minus(string command)
        {
            var exception = new ArgumentException("Should be '<name_target> = <name_source1> - <name_source_2>', but it is: " + command);
            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            var partsLeft = parts[1].Split('-').Select(item => item.Trim()).ToList();
            if (parts.Count != 2)
                throw exception;

            if (IsIdentifier(parts[0]) && IsIdentifier(partsLeft[0]) && IsIdentifier(partsLeft[1]))
                return new Plus(parts[0], partsLeft[0], partsLeft[1]);

            throw exception;
        }

        public ICommand Condition(string command)
        {
            var exception = new ArgumentException("Should be 'if <name> then', but it is: " + command);
            var parts = command.Split(' ').Select(item => item.Trim()).Where(item => string.IsNullOrEmpty(item)).ToList();
            if (parts[0] != "if")
                throw exception;
            if (IsIdentifier(parts[1]))
                new ArgumentException("Identifier expected, but have : " + parts[1]);
            if (parts[2] != "then")
                throw exception;

            return new Condition(parts[1]);
        }

        private string CheckTypeNamePair(string type, string value)
        {
            var exception = new ArgumentException(string.Format("Should be '{0} <name>', but it is: ", type) + value);

            var parts = value.Split(' ').Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
            if (parts.Count != 2)
                throw exception;
            if (parts[0] != type)
                throw exception;
            if (IsIdentifier(parts[1]))
                new ArgumentException("Identifier expected, but have : " + parts[1]);

            return parts[1];
        }

        private bool IsIdentifier(string value)
        {
            return char.IsLetter(value[0]);
        }
    }
}
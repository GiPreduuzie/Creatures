using System.Data;
using System.Linq;
using Creatures.Language.Commands;
using Creatures.Language.Commands.Interfaces;
using Creatures.ResultMonad;

namespace Creatures.Language.Parsers
{
    public class Constructor
    {
        public IResult<ICommand> NewInt(string command)
        {
            return CheckTypeNamePair("int", command).Map(x => new NewInt(x));
        }

        public IResult<ICommand> NewArray(string command)
        {
            var error = "Should be 'array <name> : v1, v2, v3...', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split(':').ToList();
            if (parts.Count != 2) return failure;

            return
                CheckTypeNamePair("array", parts[0])
                    .Map(x => new NewArray(x, parts[1].Split(',').Select(item => int.Parse(item.Trim()))));
        }

        public IResult<ICommand> CloneValue(string command)
        {
            var error = "Should be '<name_target> = <name_soure>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item=>item.Trim()).ToList();
            if (parts.Count != 2) return failure;
            if (IsIdentifier(parts[0]) && IsIdentifier(parts[1]))
                return Result<ICommand>.CreateSuccess(new CloneValue(parts[0], parts[1]));

            return failure;
        }

        public IResult<ICommand> SetValue(string command)
        {
            var error = "Should be '<name_target> = <value>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2) return failure;

            int value;
            var result = int.TryParse(parts[1], out value);
            if (!result) return Result<ICommand>.CreateFailure($"Cannot parse '{parts[1]}' as int");

            if (IsIdentifier(parts[0]))
                return Result<ICommand>.CreateSuccess(new SetValue(parts[0], value));

            return failure;
        }

        public IResult<ICommand> Plus(string command)
        {
            var error = "Should be '<name_target> = <name_source1> + <name_source_2>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2) return failure;

            var partsLeft = parts[1].Split('+').Select(item => item.Trim()).ToList();
            if (partsLeft.Count != 2) return failure;

            if (IsIdentifier(parts[0]) && IsIdentifier(partsLeft[0]) && IsIdentifier(partsLeft[1]))
                return Result<ICommand>.CreateSuccess(new Plus(parts[0], partsLeft[0], partsLeft[1]));

            return failure;
        }

        public IResult<ICommand> Minus(string command)
        {
            var error = "Should be '<name_target> = <name_source1> - <name_source_2>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item => item.Trim()).ToList();

            if (parts.Count != 2)
                return failure;

            var partsLeft = parts[1].Split('-').Select(item => item.Trim()).ToList();
            if (partsLeft.Count != 2)
                return failure;

            if (IsIdentifier(parts[0]) && IsIdentifier(partsLeft[0]) && IsIdentifier(partsLeft[1]))
                return Result<ICommand>.CreateSuccess(new Minus(parts[0], partsLeft[0], partsLeft[1]));

            return failure;
        }

        public IResult<ICommand> Print(string command)
        {
            var error = "Should be 'print <name>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split(' ').Select(item => item.Trim()).ToList();
            if (parts.Count != 2) return failure;
            if (parts[0] != "print") return failure;
            if (IsIdentifier(parts[1])) return Result<ICommand>.CreateSuccess(new Print(parts[1]));

            return failure;
        }

        public IResult<ICommand> Condition(string command)
        {
            var error = "Should be 'if <name> then', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split(' ').Select(item => item.Trim()).ToList();
            if (parts[0] != "if") return failure;
            if (!IsIdentifier(parts[1])) return Result<ICommand>.CreateFailure("Identifier expected, but have : " + parts[1]);
            if (parts[2] != "then") return failure;

            return Result<ICommand>.CreateSuccess(new Condition(parts[1]));
        }

        public IResult<ICommand> CloseCondition(string command)
        {
            var error = "Should be 'endif', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            if (command.Trim() != "endif") return failure;

            return Result<ICommand>.CreateSuccess(new CloseCondition());
        }

        public IResult<ICommand> GetState(string command)
        {
            var error = "Should be '<name_target> = getState <0..n>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2) return failure;

            var partsLeft = parts[1].Split(' ').Select(item => item.Trim()).ToList();
            if (partsLeft.Count != 2) return failure;

            if (IsIdentifier(parts[0]) && partsLeft[0] == "getState" && int.Parse(partsLeft[1]) >= 0)
                return Result<ICommand>.CreateSuccess(new GetState(parts[0], int.Parse(partsLeft[1])));

            return failure;
        }

        public IResult<ICommand> GetRandom(string command)
        {
            var error = "Should be '<name_target> = random <name_source>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Split('=').Select(item => item.Trim()).ToList();
            if (parts.Count != 2) return failure;

            var partsLeft = parts[1].Split(' ').Select(item => item.Trim()).ToList();
            if (partsLeft.Count != 2) return failure;

            if (IsIdentifier(parts[0]) && partsLeft[0] == "random" && IsIdentifier(partsLeft[1]))
                return Result<ICommand>.CreateSuccess(new GetRandom(parts[0], partsLeft[1]));

            return failure;
        }

        public IResult<ICommand> Stop(string command)
        {
            var error = "Should be 'stop', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            if (command.Trim() == "stop") return Result<ICommand>.CreateSuccess(new Stop());

            return failure;
        }

        public IResult<ICommand> SetToMemory(string command)
        {
            var error = "Should be 'setToMemory <key name> <value name>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Trim().Split(' ').Select(x => x.Trim()).ToArray();

            if (parts.Length != 3) return failure;
            if (parts[0] != "setToMemory") return failure;
            if (!IsIdentifier(parts[1])) return failure;
            if (!IsIdentifier(parts[2])) return failure;

            return Result<ICommand>.CreateSuccess(new SetToMemory(parts[1], parts[2]));
        }

        public IResult<ICommand> GetFromMemory(string command)
        {
            var error = "Should be '<target> = getFromMemory <key name>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Trim().Split('=').Select(x => x.Trim()).ToArray();

            if (parts.Length != 2) return failure;
            if (!IsIdentifier(parts[0])) return failure;

            var rightParts = parts[1].Split(' ').Select(x => x.Trim()).ToArray();
            if (rightParts[0] != "getFromMemory") return failure;
            if (!IsIdentifier(rightParts[1])) return failure;

            return Result<ICommand>.CreateSuccess(new GetFromMemory(parts[0], rightParts[1]));
        }

        public IResult<ICommand> SendMessage(string command)
        {
            var error = "Should be 'sendMessage <receiver name> <message>', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Trim().Split(' ').Select(x => x.Trim()).ToArray();

            if (parts.Length != 3) return failure;
            if (parts[0] != "sendMessage") return failure;
            if (!IsIdentifier(parts[1])) return failure;
            if (!IsIdentifier(parts[2])) return failure;

            return Result<ICommand>.CreateSuccess(new SetToMemory(parts[1], parts[2]));
        }

        public IResult<ICommand> GetFromMessageQueue(string command)
        {
            var error = "Should be '<target> = getFromMessageQueue', but it is: " + command;
            var failure = Result<ICommand>.CreateFailure(error);

            var parts = command.Trim().Split('=').Select(x => x.Trim()).ToArray();

            if (parts.Length != 2) return failure;
            if (!IsIdentifier(parts[0])) return failure;

            var rightParts = parts[1].Split(' ').Select(x => x.Trim()).ToArray();
            if (rightParts[0] != "getFromMessageQueue") return failure;

            return Result<ICommand>.CreateSuccess(new GetFromMessageQueue(parts[0]));
        }

        private IResult<string> CheckTypeNamePair(string type, string value)
        {
            var error = $"Should be '{type} <name>', but it is: " + value;
            var failure = Result<string>.CreateFailure(error);

            var parts = value.Split(' ').Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
            if (parts.Count != 2)
                return failure;
            if (parts[0] != type)
                return failure;
            if (!IsIdentifier(parts[1]))
                return Result<string>.CreateFailure("Identifier expected, but have : " + parts[1]);

            return Result<string>.CreateSuccess(parts[1]);
        }

        private bool IsIdentifier(string value)
        {
            if (value.Length == 0)
                return false;

            return value.All(x =>  char.IsLetter(x) || x == '_');
        }
    }
}
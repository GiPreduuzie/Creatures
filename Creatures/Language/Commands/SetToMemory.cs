using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    public class SetToMemory : ICommandWithArgument
    {
        public SetToMemory(string keyName, string valueName)
        {
            KeyName = keyName;
            ValueName = valueName;
        }

        public string KeyName { get; }
        public string ValueName { get; }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }

        public bool ContainsAsArgument(string variable)
        {
            return KeyName == variable || ValueName == variable;
        }
    }
}
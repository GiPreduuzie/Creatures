using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    public class GetFromMemory : ICommandSetter
    {
        public GetFromMemory(string targetName, string keyName)
        {
            TargetName = targetName;
            KeyName = keyName;
        }

        public string TargetName { get; }
        public string KeyName { get; }


        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }

        public bool ContainsAsArgument(string variable)
        {
            return KeyName == variable;
        }
    }
}
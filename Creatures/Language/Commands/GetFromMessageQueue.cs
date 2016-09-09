using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    public class GetFromMessageQueue : ICommandSetter
    {
        public GetFromMessageQueue(string targetName)
        {
            TargetName = targetName;
        }

        public string TargetName { get; }


        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }

        public bool ContainsAsArgument(string variable)
        {
            return TargetName == variable;
        }
    }
}
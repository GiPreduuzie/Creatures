using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    public class SendMessage : ICommandWithArgument
    {
        public SendMessage(string receiverPosition, string message)
        {
            ReceiverPosition = receiverPosition;
            Message = message;
        }

        public string ReceiverPosition { get; set; }
        public string Message { get; set; }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }

        public bool ContainsAsArgument(string variable)
        {
            return ReceiverPosition == variable || Message == variable;
        }
    }
}
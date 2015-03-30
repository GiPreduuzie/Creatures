namespace Creatures.Language.Commands.Interfaces
{
    public interface ICommandVisitor
    {
        void Accept(NewInt command);
        void Accept(SetValue command);
        void Accept(Plus command);
        void Accept(Print command);
        void Accept(Minus command);
    }
}
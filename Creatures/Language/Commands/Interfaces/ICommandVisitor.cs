namespace Creatures.Language.Commands
{
    internal interface ICommandVisitor
    {
        void Accept(NewInt command);
        void Accept(SetValue command);
        void Accept(Plus command);
        void Accept(Print command);
    }
}
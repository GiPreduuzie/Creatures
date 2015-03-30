namespace Creatures.Language.Commands
{
    interface ICommand
    {
        void AcceptVisitor(ICommandVisitor visitor);
    }
}
namespace Creatures.Language.Commands
{
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

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
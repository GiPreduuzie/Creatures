namespace Creatures.Language.Commands
{
    class Plus : ICommand
    {
        private readonly string _nameTarget;
        private readonly string _firstSource;
        private readonly string _secondSource;

        public Plus(string nameTarget, string firstSource, string secondSource)
        {
            _nameTarget = nameTarget;
            _firstSource = firstSource;
            _secondSource = secondSource;
        }

        public string NameTarget
        {
            get { return _nameTarget; }
        }

        public string FirstSource
        {
            get { return _firstSource; }
        }

        public string SecondSource
        {
            get { return _secondSource; }
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
namespace Creatures.Language.Commands
{
    class NewInt : ICommand
    {
        private readonly string _name;

        public NewInt(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
namespace Creatures.Language.Commands
{
    class Print : ICommand
    {
        private readonly string _variable;

        public Print(string variable)
        {
            _variable = variable;
        }

        public string Variable
        {
            get { return _variable; }
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}

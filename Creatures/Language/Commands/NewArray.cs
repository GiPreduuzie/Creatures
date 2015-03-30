using System.Collections.Generic;

namespace Creatures.Language.Commands
{
    class NewArray : ICommand
    {
        string _name;
        IEnumerable<int> _value;

        public NewArray(string name, IEnumerable<int> value)
        {
            _name = name;
            _value = value;
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
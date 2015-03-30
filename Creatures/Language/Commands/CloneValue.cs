using Creatures.Language.Commands.Interfaces;

namespace Creatures.Language.Commands
{
    class CloneValue : ICommand
    {
        string _nameTarget;
        string _nameSource;

        public CloneValue(string nameTarget, string nameSource)
        {
            _nameTarget = nameTarget;
            _nameSource = nameSource;
        }

        public void AcceptVisitor(ICommandVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
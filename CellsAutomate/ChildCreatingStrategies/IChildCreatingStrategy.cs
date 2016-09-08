using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Creatures.Language.Commands.Interfaces;

namespace CellsAutomate.ChildCreatingStrategies
{
    public interface IChildCreatingStrategy
    {
        LivegivingPrice CountPrice(int childsActionsLength);
    }
}

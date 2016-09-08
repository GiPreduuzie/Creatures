using System;

namespace CellsAutomate.Tools
{
    class ActionEx
    {
        public static ActionEnum ActionByNumber(int number)
        {
            var itemsAmount = Enum.GetNames(typeof(ActionEnum)).Length;
            switch (number % itemsAmount)
            {
                case 0: return ActionEnum.Die;
                case 1: return ActionEnum.MakeChild;
                case 2: return ActionEnum.Go;
                case 3: return ActionEnum.Eat;
                default: throw new Exception();
            }
        }
    }
}

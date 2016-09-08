using System.Linq;
using System.Text;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Parsers;
using CellsAutomate.Constants;

namespace CellsAutomate.Algorithms
{
    public static class StateMeanings
    {
        public static int UpState = 0;
        public static int RightState = 1;
        public static int DownState = 2;
        public static int LeftState = 3;

        public static int MyEnergy = 4;
        public static int FoodOnMyCell = 5;
    }


    public class ActionExperimentalAlgorithm
    {
        public ICommand[] Algorithm => new Parser().ProcessCommands(GetAlgorithm()).ToArray();

        private string GetAlgorithm()
        {
            var commands =
                new StringBuilder()
                    .AppendLine("int state")
                    .AppendLine("state = getState " + StateMeanings.MyEnergy)
                    .AppendLine("int starvingLevel")
                    .AppendLine("starvingLevel = " + CreatureConstants.ChildPrice)
                    .AppendLine("int starving")
                    .AppendLine("starving = starvingLevel - state")

                    .AppendLine("if starving then")
                    .AppendLine("    int foodOnCell")
                    .AppendLine("    foodOnCell = getState " + StateMeanings.FoodOnMyCell)
                    .AppendLine("    int haveToGo")
                    .AppendLine("    int liveCost")
                    .AppendLine("    liveCost = " + CreatureConstants.MinFoodToSurvive)
                    .AppendLine("    haveToGo = liveCost - foodOnCell")

                    .AppendLine("    if haveToGo then")
                    .AppendLine("        int go")
                    .AppendLine("        go = " + (int)ActionEnum.Go)
                    .AppendLine("        int randomGo")
                    .AppendLine("        randomGo = random state")
                    .AppendLine("        print go")
                    .AppendLine("        print randomGo")
                    .AppendLine("        stop")
                    .AppendLine("    endif")

                    .AppendLine("    int eat")
                    .AppendLine("    eat = " + (int)ActionEnum.Eat)
                    .AppendLine("    print eat")
                    .AppendLine("    print state")
                    .AppendLine("    stop")
                    .AppendLine("endif")

                    .AppendLine("int child")
                    .AppendLine("child = " + (int)ActionEnum.MakeChild)
                    .AppendLine("print child")
                    .AppendLine("print state");

            return commands.ToString();
        }
    }


    public class ActionAlgorithm
    {
        public ICommand[] Algorithm => new Parser().ProcessCommands(GetAlgorithm()).ToArray();

        private string GetAlgorithm()
        {
            var commands =
                new StringBuilder()
                    // зададим константы
                    .AppendLine("int zero")
                    .AppendLine("int one")
                    .AppendLine("int two")
                    .AppendLine("int three")
                    .AppendLine("int four")

                    .AppendLine("zero  = 0")
                    .AppendLine("one   = 1")
                    .AppendLine("two   = 2")
                    .AppendLine("three = 3")
                    .AppendLine("four  = 4")

                    .AppendLine("int hasToEat")
                    .AppendLine("int hasOneBite")
                    .AppendLine("int canMakeChild")

                    .AppendLine("hasToEat = getState 0")
                    .AppendLine("hasOneBite = getState 1")
                    .AppendLine("canMakeChild = getState 2")

                    .AppendLine("if hasToEat then")
                    .AppendLine("if hasOneBite then")
                    .AppendLine("print three")
                    .AppendLine("stop")
                    .AppendLine("endif")
                    .AppendLine("print two")
                    .AppendLine("stop")
                    .AppendLine("endif")

                    .AppendLine("if canMakeChild then")
                    .AppendLine("print one")
                    .AppendLine("stop")
                    .AppendLine("endif")

                    .AppendLine("int returnAction")
                    .AppendLine("returnAction = random two")
                    .AppendLine("returnAction = returnAction + one")
                    .AppendLine("print returnAction")
                    .AppendLine("stop");

            return commands.ToString();
        }
    }
}

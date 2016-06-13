using Creatures.Language.Executors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creaturestests
{
    public class TestExecutorToolset : IExecutorToolset
    {
        Tuple<int, int>[] _randoms;
        IDictionary<int, int> _state;
        int _randomCounter = 0;

        public TestExecutorToolset(Tuple<int, int>[] randoms, IDictionary<int, int> state)
        {
            _randoms = randoms;
            _state = state;
        }

        public int GetRandom(int maxValue)
        {
            Assert.AreEqual(maxValue, _randoms[_randomCounter].Item1);
            var result =  _randoms[_randomCounter].Item2;
            _randomCounter++;
            return result;
        }

        public int GetState(int direction)
        {
            return _state[direction];
        }
    }

    [TestClass]
    public class StartAlgorithTests : TestsBase
    {
        [TestMethod]
        public void AllCellsAreOpen()
        {
            var state = new Dictionary<int, int>() {
                { 0, 4 },
                { 1, 4 },
                { 2, 4 },
                { 3, 4 } };

            foreach (var i in new []{1, 2, 3, 4})
            {
                var randomChoice = new[] { Tuple.Create(4, i) };

                Check(GetAlgorithm().ToString(), new TestExecutorToolset(randomChoice, state), i);
            }
        }

        [TestMethod]
        public void AllCellsAreClosed()
        {
            var state = new Dictionary<int, int>() {
                { 0, 3 },
                { 1, 3 },
                { 2, 3 },
                { 3, 3 } };

            var randomChoice = new Tuple<int, int> [] { };

            Check(GetAlgorithm().ToString(), new TestExecutorToolset(randomChoice, state), 0);
        }

        [TestMethod]
        public void OneCellIsOpen()
        {
            foreach (var i in new[] { 1, 2, 3, 4 })
            {
                var state = new[] { 0, 1, 2, 3 }.ToDictionary(x => x, x => x == (i - 1) ? 4 : 3);

                var randomChoice = new[] { Tuple.Create(1, 1) };

                Check(GetAlgorithm().ToString(), new TestExecutorToolset(randomChoice, state), i);
            }
        }

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

                // возьмём состояние вокруг
                .AppendLine("int upState")
                .AppendLine("int rightState")
                .AppendLine("int downState")
                .AppendLine("int leftState")
                .AppendLine("upState    = getState 0")
                .AppendLine("rightState = getState 1")
                .AppendLine("downState  = getState 2")
                .AppendLine("leftState  = getState 3")

                // посчитаем сколько свободных клеток
                .AppendLine("int directionsToGo")
                .AppendLine("directionsToGo = 0")

                .AppendLine("int upState_isEmpty")
                .AppendLine("int rightState_isEmpty")
                .AppendLine("int downState_isEmpty")
                .AppendLine("int leftState_isEmpty")

                .AppendLine("upState_isEmpty    = upState    - four")
                .AppendLine("rightState_isEmpty = rightState - four")
                .AppendLine("downState_isEmpty  = downState  - four")
                .AppendLine("leftState_isEmpty  = leftState  - four")

                .AppendLine("if upState_isEmpty then")
                .AppendLine("directionsToGo = directionsToGo + one")
                .AppendLine("endif")

                .AppendLine("if rightState_isEmpty then")
                .AppendLine("directionsToGo = directionsToGo + one")
                .AppendLine("endif")

                .AppendLine("if downState_isEmpty then")
                .AppendLine("directionsToGo = directionsToGo + one")
                .AppendLine("endif")

                .AppendLine("if leftState_isEmpty then")
                .AppendLine("directionsToGo = directionsToGo + one")
                .AppendLine("endif")

                //Если некуда идти, выведем 0, т.е. "стой на месте"
                .AppendLine("int minus_directionsToGo")
                .AppendLine("minus_directionsToGo = zero - directionsToGo")
                .AppendLine("if directionsToGo then")
                .AppendLine("if minus_directionsToGo then")
                .AppendLine("print zero")
                .AppendLine("stop")
                .AppendLine("endif")
                .AppendLine("endif")

                //Решим куда шагнуть
                .AppendLine("int selectedCell")
                .AppendLine("selectedCell = random directionsToGo")

                //Шагнём
                .AppendLine("int counter")
                .AppendLine("counter = zero")
                .AppendLine("int isThisCell")

                .AppendLine("if upState_isEmpty then")
                .AppendLine("counter = counter + one")
                .AppendLine("isThisCell = counter - selectedCell")
                .AppendLine("if isThisCell then")
                .AppendLine("print one")
                .AppendLine("stop")
                .AppendLine("endif")
                .AppendLine("endif")

                .AppendLine("if rightState_isEmpty then")
                .AppendLine("counter = counter + one")
                .AppendLine("isThisCell = counter - selectedCell")
                .AppendLine("if isThisCell then")
                .AppendLine("print two")
                .AppendLine("stop")
                .AppendLine("endif")
                .AppendLine("endif")

                .AppendLine("if downState_isEmpty then")
                .AppendLine("counter = counter + one")
                .AppendLine("isThisCell = counter - selectedCell")
                .AppendLine("if isThisCell then")
                .AppendLine("print three")
                .AppendLine("stop")
                .AppendLine("endif")
                .AppendLine("endif")

                .AppendLine("if leftState_isEmpty then")
                .AppendLine("counter = counter + one")
                .AppendLine("isThisCell = counter - selectedCell")
                .AppendLine("if isThisCell then")
                .AppendLine("print four")
                .AppendLine("stop")
                .AppendLine("endif")
                .AppendLine("endif");

            return commands.ToString();
        }
    }
}

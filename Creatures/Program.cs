using System;
using System.Text;
using Creatures.Language.Commands;
using Creatures.Language.Executors;
using Creatures.Language.Parsers;

namespace Creatures
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands =
                new StringBuilder()
                    .AppendLine("int a")
                    .AppendLine("a=1")
                    .AppendLine("int b")
                    .AppendLine("b=2")
                    .AppendLine("a=a+b")
                    .AppendLine("print a");

            var parsedCommands = new Constructor().ProcessCommands(commands.ToString());
            Console.WriteLine(new Executor().Execute(parsedCommands));
        }
    }
}

﻿using System;
using System.Linq;
using System.Text;
using Creatures.Language.Executors;
using Creatures.Language.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreaturesTests
{
    [TestClass]
    public class LanguageTests
    {
        [TestMethod]
        public void Plus()
        {
            var commands =
               new StringBuilder()
                   .AppendLine("int a")
                   .AppendLine("a=1")
                   .AppendLine("int b")
                   .AppendLine("b=2")
                   .AppendLine("a=a+b")
                   .AppendLine("print a");

            Check(commands.ToString(), 3);
        }

        [TestMethod]
        public void Minus()
        {
            var commands =
               new StringBuilder()
                   .AppendLine("int a")
                   .AppendLine("a=1")
                   .AppendLine("int b")
                   .AppendLine("b=2")
                   .AppendLine("a=a-b")
                   .AppendLine("print a");

            Check(commands.ToString(), -1);
        }

        private void Check(string commands, params int[] values)
        {
            var parsedCommands = new Constructor().ProcessCommands(commands);
            var output =
                new Executor().Execute(parsedCommands)
                    .Replace("\r\n", "\n")
                    .Split('\n')
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Select(int.Parse)
                    .ToList();

            Assert.AreEqual(values.Count(), output.Count());

            for (var i = 0; i < values.Count(); i++)
            {
                Assert.AreEqual(values[i], output[i]);
            }
        }
    }
}

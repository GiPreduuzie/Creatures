﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using CellsAutomate;
using CellsAutomate.Algorithms;
using CellsAutomate.Constants;
using CellsAutomate.Creatures;
using Creatures.Language.Commands.Interfaces;
using Creatures.Language.Parsers;
using Matrix = CellsAutomate.Matrix;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        DependenciesResolver.DependencyResolver _dependenciesResolver = new DependenciesResolver.DependencyResolver();

        private DispatcherTimer _timer;
        private Matrix _matrix;
        private Random _random = new Random();
        private int _step = 0;
        private Stopwatch _calculateTimer = new Stopwatch();
        private Stopwatch _paintTimer = new Stopwatch();

        private readonly Rectangle[,] _squares;
        private TurnExecutor _turnExecutor;

        public MainWindow()
        {
            InitializeComponent();

            var size = _dependenciesResolver.GetMatrixSize();

            var width = size;
            var height = size;

            _squares = new Rectangle[width, height];
            WorkWirhGrid.MarkTable(MainGrid, width, height);
            WorkWirhGrid.InitalizeGrid(MainGrid, _squares, width, height);

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, LogConstants.TimeSpanSeconds, LogConstants.TimeSpanMSeconds)
            };
            _timer.Tick += PrintCurrentMatrix;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            var matrixSize = _dependenciesResolver.GetMatrixSize();

            var commandsForGetDirection = new GetDirectionAlgorithm().Algorithm;
            var commandsForGetAction = new GetActionAlgorithm().Algorithm;
            var creator = new CreatorOfCreature(commandsForGetAction, commandsForGetDirection);

            _matrix = _dependenciesResolver.GetMatrix();
            _matrix.FillStartMatrixRandomly();

            Print(_step, matrixSize, _matrix);

            _turnExecutor = new TurnExecutor(_matrix);
        }
        
        private async void MakeTurn(object sender, object o)
        {
            _step++;
            SetWindowTitle(_step);
            if (_matrix.AliveCount == 0)
            {
                _timer.Stop();
            }

            _calculateTimer.Start();
            await Task.Factory.StartNew(_matrix.MakeTurn);
            _calculateTimer.Stop();

            Print(_step, _dependenciesResolver.GetMatrixSize(), _matrix);
        }

        private void PrintCurrentMatrix(object sender, object o)
        {
            _turnExecutor.Stop();

            SetWindowTitle(_turnExecutor.Steps);

            Print(_turnExecutor.Steps, _dependenciesResolver.GetMatrixSize(), _matrix);

            if (_matrix.AliveCount == 0)
            {
                _timer.Stop();
                _turnExecutor.Stop();
                return;
            }

            _turnExecutor.Start();
        }

        private void SetWindowTitle(int step)
        {
            Window.Title = $"Step: {step} / Calc time: {Math.Round(_calculateTimer.Elapsed.TotalSeconds, 2)} / Paint time: {Math.Round(_paintTimer.Elapsed.TotalSeconds, 2)}";
        }

        private string CreateLogOfCreature(Creature creature, int generation)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Generation: {generation}");
            builder.AppendLine("- - - - - ActionCommands - - - - -");
            builder.AppendLine(CommandsToString(creature.CommandsForGetAction));
            builder.AppendLine("- - - - - DirectionCommands - - - - -");
            builder.AppendLine(CommandsToString(creature.CommandsForGetDirection));
            return builder.ToString();
        }

        private string CommandsToString(ICommand[] commands)
        {
            var builder = new StringBuilder();
            var commandsToString = new CommandToStringParser();
            for (int num = 0; num < commands.Length; num++)
            {
                builder.AppendLine($"{num}) " + commandsToString.ParseCommand(commands[num]));
            }
            return builder.ToString();
        }

        private T ChooseRandom<T>(IList<T> collection)
        {
            return collection[_random.Next(collection.Count)];
        }

        private void PrintGeneration(Matrix creatures, int turn)
        {
            for (int i = 1; i <= turn + 1; i++)
            {
                var g = (from x in creatures.CreaturesAsEnumerable where x.Generation == i select x).Count();
                if (g != 0)
                    Console.WriteLine(i + "=> " + g);
            }
        }

        private void Print(int id, int length, Matrix matrix)
        {
            _paintTimer.Start();
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var isThereAreFood = matrix.EatMatrix.HasOneBite(new System.Drawing.Point(i, j));
                    var isThereAreCreature = matrix.Creatures[i, j] != null;
                    var strokeColor = new Color();
                    var fillColor = new Color();

                    if (isThereAreCreature)
                    {
                        fillColor = Colors.Black;
                        strokeColor = isThereAreFood ? Colors.YellowGreen : Colors.OrangeRed;
                    }
                    else
                    {
                        if (isThereAreFood)
                        {
                            fillColor = Color.FromArgb(50,154,205,50);
                            strokeColor = fillColor;
                        }
                        else
                        {
                            fillColor = Colors.White;
                            strokeColor = fillColor;
                        }
                    }

                    PaintSquareStroke(i, j, strokeColor, _squares);
                    PaintSquareFill(i, j, fillColor, _squares);
                }
            }
            _paintTimer.Stop();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (_turnExecutor.IsRunning)
            {
                _turnExecutor.Stop();
                _timer.Stop();
                _calculateTimer.Stop();
            }
            else
            {
                _turnExecutor.Start();
                _timer.Start();
                _calculateTimer.Start();
            }
        }

        private void onestep_Click(object sender, RoutedEventArgs e)
        {
            MakeTurn(null, null);
        }
    }
}

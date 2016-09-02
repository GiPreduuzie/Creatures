using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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
        private int _step = 0;
        private Stopwatch _calculateTimer = new Stopwatch();
        private Stopwatch _paintTimer = new Stopwatch();

        private TurnExecutor _turnExecutor;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, LogConstants.TimeSpanSeconds, LogConstants.TimeSpanMSeconds)
            };
            _timer.Tick += PrintCurrentMatrix;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            var matrixSize = _dependenciesResolver.GetMatrixSize();
            
            _matrix = _dependenciesResolver.GetMatrix();
            _matrix.FillStartMatrixRandomly();

            PrintBitmap(matrixSize, _matrix);
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

            PrintBitmap(_dependenciesResolver.GetMatrixSize(), _matrix);
        }

        private void PrintCurrentMatrix(object sender, object o)
        {
            if (MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Stop();
            }

            SetWindowTitle(_turnExecutor.Steps);

            PrintBitmap(_dependenciesResolver.GetMatrixSize(), _matrix);

            if (_matrix.AliveCount == 0)
            {
                _timer.Stop();
                _turnExecutor.Stop();
                return;
            }
            if (MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Start();
            }
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

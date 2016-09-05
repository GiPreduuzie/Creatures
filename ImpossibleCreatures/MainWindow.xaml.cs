using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CellsAutomate.Constants;
using Matrix = CellsAutomate.Matrix;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private VisualizationType _visualizationType = VisualizationType.CanEat;

        private int _nationsCount = 0;
        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _calculateTimer = new Stopwatch();
        private readonly Stopwatch _paintTimer = new Stopwatch();
        private TurnExecutor _turnExecutor;

        private readonly Lazy<Matrix> _matrix = new Lazy<Matrix>(() => new DependenciesResolver.DependencyResolver().GetMatrix());


        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, LogConstants.TimeSpanMSeconds)
            };

            _timer.Tick += PrintCurrentMatrix;
           
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            _matrix.Value.FillStartMatrixRandomly();
            PrintBitmap();
            _turnExecutor = new TurnExecutor(_matrix.Value);
        }
        
        private async void MakeTurn(object sender, object o)
        {
            _turnExecutor.Steps++;
            ShowInfo();
            if (_matrix.Value.AliveCount == 0)
            {
                _timer.Stop();
            }

            _calculateTimer.Start();
            await Task.Factory.StartNew(_matrix.Value.MakeTurn);
            _calculateTimer.Stop();

            if (_nationsCount == 1)
            {
                MarkParts();
            }

            PrintBitmap();
        }

        private void MarkParts()
        {
            Debug.WriteLine("Mark parent");

            var n = 0;
            if (_matrix.Value.GetNationsAmount() == 1)
            {
                _colorsManager.Reset();

                foreach (var creature in _matrix.Value.CreaturesAsEnumerable)
                {
                    creature.ParentMark = n;
                    n++;
                }
            }
        }

        private void PrintCurrentMatrix(object sender, object o)
        {
            if ((bool) MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Stop();
            }

            ShowInfo();

            if (_nationsCount == 1)
            {
                MarkParts();
            }

            PrintBitmap();

            if (_matrix.Value.AliveCount == 0)
            {
                _timer.Stop();
                _turnExecutor.Stop();
                return;
            }
            if ((bool) MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Start();
            }
        }

        private void ShowInfo()
        {
            StepCount.Content = "Step: " + _turnExecutor.Steps;
            CalcTime.Content = "Calc time: " + Math.Round(_calculateTimer.Elapsed.TotalSeconds, 1) + "s";
            PaintTime.Content = "Paint time: " + Math.Round(_paintTimer.Elapsed.TotalSeconds, 1) + "s";

            _nationsCount = _matrix.Value.GetNationsAmount();

            NationCount.Content = "Nation: " + _nationsCount;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
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

        private void OneStep_Click(object sender, RoutedEventArgs e)
        {
            MakeTurn(null, null);
        }
    }
}

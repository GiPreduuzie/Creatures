using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CellsAutomate;
using CellsAutomate.Constants;
using Color = System.Windows.Media.Color;
using Matrix = CellsAutomate.Matrix;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private VisualizationType _visualizationType = VisualizationType.CanEat;

        readonly DependenciesResolver.DependencyResolver _dependenciesResolver = new DependenciesResolver.DependencyResolver();

        private readonly DispatcherTimer _timer;
        private Matrix _matrix;
        private readonly Stopwatch _calculateTimer = new Stopwatch();
        private readonly Stopwatch _paintTimer = new Stopwatch();

        private TurnExecutor _turnExecutor;

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
            _matrix = _dependenciesResolver.GetMatrix();
            _matrix.FillStartMatrixRandomly();

            PrintBitmap();
            _turnExecutor = new TurnExecutor(_matrix);
        }
        
        private async void MakeTurn(object sender, object o)
        {
            _turnExecutor.Steps++;
            SetWindowTitle();
            if (_matrix.AliveCount == 0)
            {
                _timer.Stop();
            }

            _calculateTimer.Start();
            await Task.Factory.StartNew(_matrix.MakeTurn);
            _calculateTimer.Stop();

            if (_turnExecutor.Steps % LogConstants.StepsBetweenColorChange == 0)
            {
                MarkParts(_dependenciesResolver.GetMatrixSize());
            }

            PrintBitmap();
        }

        private void MarkParts(int matrixSize)
        {
            Debug.WriteLine("Mark parent");

            var list = new List<Membrane>();
            for (var i = 0; i < matrixSize; i++)
            {
                for (var j = 0; j < matrixSize; j++)
                {
                    var creature = _matrix.Creatures[i, j];

                    if (creature != null)
                    {
                        list.Add(creature);
                    }
                }
            }

            int n = 0;

            if (list.Select(x => x.ParentMark).Distinct().Count() == 1)
            {
                _colorsManager.Reset();

                for (var i = 0; i < matrixSize; i++)
                {
                    for (var j = 0; j < matrixSize; j++)
                    {
                        var creature = _matrix.Creatures[i, j];

                        if (creature != null)
                        {
                            creature.ParentMark = n;
                            n++;
                        }
                    }
                }
            }
        }

        private int _stepToMarkParts = 0;
        private void PrintCurrentMatrix(object sender, object o)
        {
            if ((bool) MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Stop();
            }

            _stepToMarkParts++;
            SetWindowTitle();

            if (_turnExecutor.Steps >= _stepToMarkParts)
            {
                MarkParts(_dependenciesResolver.GetMatrixSize());
                _stepToMarkParts += LogConstants.StepsBetweenColorChange;
            }

            PrintBitmap();

            if (_matrix.AliveCount == 0)
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

        private void SetWindowTitle()
        {
            StepCount.Content = "Step: " + _turnExecutor.Steps;
            CalcTime.Content = "Calc time: " + Math.Round(_calculateTimer.Elapsed.TotalSeconds, 1) + "s";
            PaintTime.Content = "Paint time: " + Math.Round(_paintTimer.Elapsed.TotalSeconds, 1) + "s";
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

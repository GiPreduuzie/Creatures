using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CellsAutomate;
using ImpossibleCreatures.Settings;
using Matrix = CellsAutomate.Matrix;

namespace ImpossibleCreatures
{
    public partial class MainWindow : Window
    {
        private VisualizationType _visualizationType = VisualizationType.CanEat;

        private int _nationsCount;
        private readonly DispatcherTimer _timer;
        private double _averageGenotypeLength;

        private readonly Stopwatch _paintTimer = new Stopwatch();
        private TurnExecutor _turnExecutor;
        public ExecutionSettings ExecutionSettings { get; set; } = new ExecutionSettings();

        private Matrix _matrix;
        private Matrix Matrix => _matrix ?? (_matrix = GetMatrix());

        private Matrix GetMatrix()
        {
            var settingsManager = new SettingsManager();

            return new Matrix(
                settingsManager.GetMatrixSize(ComboBoxMatrixSize),
                settingsManager.GetMatrixSize(ComboBoxMatrixSize),
                settingsManager.GetCreatureCreator(ComboBoxChildCreationPrice),
                settingsManager.GetFoodDistributionStrategy(ComboBoxFoodDistibutionStrategy),
                settingsManager.GetFoodBehavior(ComboBoxFoodBehavior),
                settingsManager.GetFoodDistributingFrequency(SliderFoodDistributingFrequency),
                settingsManager.GetChildCreatingStrategy(ComboBoxChildCreationPrice));
        }

        public MainWindow()
        {
            InitializeComponent();

            FillCombobox(ComboBoxMatrixSize, new[] { "100", "150", "200", "250", "300" });
            ComboBoxMatrixSize.SelectionChanged += ComboBox_SelectionChanged;

            FillCombobox(ComboBoxChildCreationPrice, Enum.GetNames(typeof(ChildCreationPrice)));
            ComboBoxChildCreationPrice.SelectionChanged += ComboBox_SelectionChanged;

            FillCombobox(ComboBoxFoodDistibutionStrategy, Enum.GetNames(typeof(FoodDistibutionStrategy)));
            ComboBoxFoodDistibutionStrategy.SelectionChanged += ComboBox_SelectionChanged;

            FillCombobox(ComboBoxFoodBehavior, Enum.GetNames(typeof(FoodBehavior)));
            ComboBoxFoodBehavior.SelectionChanged += ComboBox_SelectionChanged;


            var time = (int)RefreshSpeed.Value;

            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, time)
            };

            RefreshSpeed.Value = time;

            _timer.Tick += PrintCurrentMatrix;

        }

        private void FillCombobox(ComboBox combobox, string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                combobox.Items.Add(new ComboBoxItem { Content = items[i], IsSelected = i == 0 });
            }
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            Matrix.FillStartMatrixRandomly();
            PrintBitmap();
            _turnExecutor = new TurnExecutor(Matrix, ExecutionSettings);
        }

        private async void MakeTurn(object sender, object o)
        {
            await Task.Factory.StartNew(_turnExecutor.RunSingleTurn);

            ProcessCurrentStatus();
        }

        private void MarkParts()
        {
            Debug.WriteLine("Mark parent");

            var n = 0;

            _colorsManager.Reset();

            foreach (var creature in Matrix.CreaturesAsEnumerable)
            {
                creature.ParentMark = n;
                n++;
            }

        }

        private void PrintCurrentMatrix(object sender, object o)
        {


            if ((bool)MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Stop();
            }

            if (!ProcessCurrentStatus())
                return;

            if ((bool)MenuItemSyncRendering.IsChecked)
            {
                _turnExecutor.Start();
            }
        }

        private bool ProcessCurrentStatus()
        {
            ShowInfo();

            if (_nationsCount == 1)
            {
                MarkParts();
            }

            PrintBitmap();

            if (Matrix.AliveCount == 0)
            {
                _timer.Stop();
                _turnExecutor.Stop();
                return false;
            }

            return true;
        }

        private void ShowInfo()
        {
            StepCount.Content = $"Step: {_turnExecutor.Steps}";
            CalcTime.Content = $"Calc time: {Math.Round(_turnExecutor.LastTurnTook.TotalMilliseconds, 1)}ms";
            PaintTime.Content = $"Paint time: {Math.Round(_paintTimer.Elapsed.TotalMilliseconds, 1)}ms";

            _nationsCount = Matrix.GetNationsAmount();

            var creatures = Matrix.CreaturesAsEnumerable;
            _averageGenotypeLength = creatures.Any() ? creatures.Select(x => x.Creature.GenotypeLength).Average() : 0;

            NationCount.Content = "Nation: " + _nationsCount;
            AverageGenotypeLength.Content = "Avg.gen: " + Math.Round(_averageGenotypeLength, 1);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (_turnExecutor.IsRunning)
            {
                _turnExecutor.Stop();
                _timer.Stop();
                ProcessCurrentStatus();
                ButtonStartStop.Content = "Start";
                ButtonOneStep.IsEnabled = true;
            }
            else
            {
                _turnExecutor.Start();
                _timer.Start();
                ButtonStartStop.Content = "Stop";
                ButtonOneStep.IsEnabled = false;
            }
        }

        private void OneStep_Click(object sender, RoutedEventArgs e)
        {
            MakeTurn(null, null);
        }

        private void Repaint_Click(object sender, RoutedEventArgs e)
        {
            _turnExecutor.Stop();

            MarkParts();
            PrintBitmap();

            _turnExecutor.Start();
        }

        private void RefreshSpeed_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
            }
        }

        private void Visualisavion_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton)sender;
            _visualizationType = (VisualizationType)Enum.Parse(typeof(VisualizationType), radioButton.Tag.ToString());
            ProcessCurrentStatus();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            _turnExecutor.Stop();
            _timer.Stop();

            _nationsCount = 0;
            _averageGenotypeLength = 0;

            _matrix = GetMatrix();

            Matrix.FillStartMatrixRandomly();
            PrintBitmap();
            _turnExecutor = new TurnExecutor(Matrix, ExecutionSettings);

            _turnExecutor.Start();
            _timer.Start();

            ButtonStartStop.Content = "Stop";

            ButtonStartStop.IsEnabled = true;
            ButtonOneStep.IsEnabled = true;
            ButtonRepaint.IsEnabled = true;
        }

        private void PaintGrid_Click(object sender, RoutedEventArgs e)
        {
            PrintBitmap();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonStartStop.IsEnabled = false;
            ButtonOneStep.IsEnabled = false;
            ButtonRepaint.IsEnabled = false;
        }

        private void SliderFoodDistributingFrequency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //ComboBox_SelectionChanged(null, null);
        }
    }
}
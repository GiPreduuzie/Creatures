using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CellsAutomate;

namespace ImpossibleCreatures
{
    public class TurnExecutor
    {
        private readonly Matrix Matrix;
        private readonly ExecutionSettings _settings;
        private bool _stopping;
        private Task _task;
        public int Steps { get; set; }
        public TimeSpan LastTurnTook { get; private set; }

        public TurnExecutor(Matrix matrix, ExecutionSettings settings)
        {
            Matrix = matrix;
            _settings = settings;
        }

        public void Start()
        {
            _task = new Task(RunTurns);
            _task.Start();
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _stopping = true;
                _task.Wait();
            }
        }

        public bool IsRunning => _task != null && _task.Status == TaskStatus.Running;

        private void RunTurns()
        {
            while (!_stopping)
            {
                RunSingleTurn();
            }

            _stopping = false;
        }

        public void RunSingleTurn()
        {
            var timer = Stopwatch.StartNew();

            Matrix.MakeTurn(_settings);

            LastTurnTook = timer.Elapsed;

            Steps++;
        }
    }
}
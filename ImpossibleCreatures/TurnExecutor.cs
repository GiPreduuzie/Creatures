using System.Threading.Tasks;
using CellsAutomate;

namespace ImpossibleCreatures
{
    public class TurnExecutor
    {
        private readonly Matrix _matrix;
        private bool _stopping;
        private Task _task;
        public int Steps { get; set; }

        public TurnExecutor(Matrix matrix)
        {
            _matrix = matrix;
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
                _matrix.MakeTurn();
                Steps++;
            }

            _stopping = false;
        }
    }
}
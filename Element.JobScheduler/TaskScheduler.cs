using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Element.JobScheduler
{
    public class TaskScheduler : System.Threading.Tasks.TaskScheduler, IDisposable
    {
        private readonly BlockingCollection<Task> _tasks = new BlockingCollection<Task>();
        private readonly Thread mainThread = null;

        public TaskScheduler()
        {
            mainThread = new Thread(new ThreadStart(Execute));
            if (!mainThread.IsAlive)
            {
                mainThread.Start();
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public void Dispose()
        {
            _tasks.CompleteAdding();
            _tasks.Dispose();

            mainThread?.Abort();

            GC.SuppressFinalize(this);
        }

        private void Execute()
        {
            foreach (var task in _tasks.GetConsumingEnumerable())
            {
                TryExecuteTask(task);
            }
        }
    }
}

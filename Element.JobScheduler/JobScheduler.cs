using Cronos;
using Element.JobScheduler.Interfaces;
using Element.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Element.JobScheduler
{
    internal class JobScheduler : IJobScheduler, IDisposable
    {
        private readonly JobSchedulerConfiguration _config = new JobSchedulerConfiguration();
        private readonly TaskScheduler _taskScheduler = new TaskScheduler();
        private readonly Dictionary<string, Timer> _timersDictionary = new Dictionary<string, Timer>();

        private Action<string, string, DateTime?> _onJobUpdate;

        public JobScheduler(Action<JobSchedulerConfiguration> config)
        {
            config.Invoke(_config);

            _onJobUpdate = (name, cron, date) => _config.StorageProvider?.SaveJob(new JobInfo { Name = name, CronExpression = cron, LastSuccessfulRun = date });
        }

        public IJobScheduler Execute<T>() where T : IScheduledJob
        {
            var name = typeof(T).Name;
            Task.Factory.StartNew(() =>
            {
                _config.OnJobStart?.Invoke(name);
                Activator.CreateInstance<T>().Execute();
            }, CancellationToken.None, TaskCreationOptions.LongRunning, _taskScheduler)
                .ContinueWith(task =>
                {
                    ContinueWith(task, name);
                });

            return this;
        }

        public IJobScheduler Execute(Action action)
        {
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.LongRunning, _taskScheduler)
                .ContinueWith(task =>
                {
                    ContinueWith(task);
                });

            return this;
        }

        public IJobScheduler ScheduleJob<T>(string cronExpression) where T : IScheduledJob
        {
            var name = typeof(T).Name;
            if (_timersDictionary.ContainsKey(name))
            {
                throw new Exception($"There's already a job with the name: {name}");
            }

            var timer = GetJobTimer(name, () =>
            {
                _config.OnJobStart?.Invoke(name);
                Activator.CreateInstance<T>().Execute();
            }, cronExpression);

            _timersDictionary.Add(name, timer);

            return this;
        }

        public void Dispose()
        {
            _taskScheduler?.Dispose();
            _timersDictionary?.Clear();

            GC.SuppressFinalize(this);
        }

        private Timer GetJobTimer(string name, Action action, string cronExpression)
        {
            CronExpression expression = CronExpression.Parse(cronExpression, CronFormat.Standard);
            DateTimeOffset next = expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local) ?? DateTimeOffset.Now;
            DateTimeOffset nextAfterNext = expression.GetNextOccurrence(next, TimeZoneInfo.Local) ?? next;

            var startTime = next - DateTimeOffset.Now;
            var period = nextAfterNext - next;

            var timer = new Timer((callback) =>
            {
                Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.LongRunning, _taskScheduler)
                .ContinueWith(task => ContinueWith(task, name, cronExpression));
            }, null, (long)startTime.TotalMilliseconds, (long)period.TotalMilliseconds);

            _onJobUpdate?.Invoke(name, cronExpression, null);

            return timer;
        }

        public void ContinueWith(Task task, string jobName, string cronExpression)
        {
            bool hasErrors = task.Exception != null;

            _config.StorageProvider?.AddHistory(new JobHistoryInfo
            {
                JobName = jobName,
                Description = hasErrors ? $"Job failed: {task.Exception}" : "Job executed successfuly",
                ExecutionDate = DateTime.Now,
                RunSuccessfuly = !hasErrors
            });

            if (hasErrors)
            {
                _config.OnErrorCallback?.Invoke(task.Exception);
                return;
            }

            _config.OnJobEnd?.Invoke(jobName);
            _onJobUpdate?.Invoke(jobName, cronExpression, DateTime.Now);
        }

        private void ContinueWith(Task task, string jobName = "Anonymous execution")
        {
            if (task.Exception != null)
            {
                _config.OnErrorCallback?.Invoke(task.Exception);
                return;
            }

            _config.OnJobEnd?.Invoke(jobName);
        }
    }
}

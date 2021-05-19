using Element.JobScheduler.Interfaces;
using Element.JobScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Element.JobScheduler
{
    internal class JobScheduler : IJobScheduler, IDisposable
    {
        private readonly JobSchedulerConfiguration _config;
        private readonly IDisposableDictionary<Type, JobTimer> _timersDict;

        public JobScheduler(Action<JobSchedulerConfiguration> config)
        {
            _config = new JobSchedulerConfiguration();
            _timersDict = new DisposableConcurrentDictionary<Type, JobTimer>();

            config.Invoke(_config);
        }

        public IJobScheduler Execute(Action action)
        {
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.LongRunning, new TaskScheduler());

            return this;
        }

        public void Trigger(string job)
        {
            GetJobTimer(job)?.Trigger();
        }

        public IJobScheduler ScheduleJob<T>(string cronExpression) where T : IScheduledJob
        {
            var type = typeof(T);
            if (_timersDict.ContainsKey(type))
            {
                throw new Exception($"There's already a job with the name: {type.Name}");
            }

            var timer = JobTimer.New<T>(cronExpression, _config);

            _timersDict.Add(type, timer);

            return this;
        }

        public void Cancel(string job)
        {
            var timer = GetJobTimer(job);
            if (timer == null || !timer.IsRunning)
            {
                return;
            }

            timer.Cancel();
        }

        public IEnumerable<JobModel> GetJobs()
        {
            return _timersDict.Select(x => new JobModel
            {
                Name = x.Key.Name,
                LastCancelledDate = x.Value.LastCancelled,
                LastRunDate = x.Value.LastRun,
                LastExecutionDuration = x.Value.LastExecutionDuration,
                RunWithSuccess = x.Value.RunWithSuccess,
                NextRunDate = x.Value.NextRun,
                IsRunning = x.Value.IsRunning,
                IsCancelling = x.Value.IsCancelling
            }).OrderBy(x => x.NextRunDate);
        }

        public void Dispose()
        {
            _timersDict?.Dispose();

            GC.SuppressFinalize(this);
        }

        private JobTimer GetJobTimer(string job)
        {
            var jobType = _timersDict.SingleOrDefault(x => x.Key.Name.Equals(job));
            return Equals(jobType, default(KeyValuePair<Type, JobTimer>))
                ? null
                : jobType.Value;
        }
    }
}

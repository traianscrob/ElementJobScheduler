using Element.JobScheduler.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Element.JobScheduler
{
    internal class JobTimer : IDisposable
    {
        private readonly JobSchedulerConfiguration _config;
        private readonly CronExpressionWrapper _cronWrapper;
        private readonly Timer _timer;

        private DateTimeOffset? _startTime;
        private JobCancellationTokenSource _cancellationToken;
        private bool _manuallyTriggered;

        public DateTime? LastRun { get; private set; }
        public DateTime? LastCancelled { get; private set; }
        public bool? RunWithSuccess { get; private set; }
        public TimeSpan? LastExecutionDuration { get; set; }
        public bool IsRunning { get; private set; }
        public bool IsCancelling { get; private set; }

        public DateTimeOffset NextRun => _cronWrapper.GetNextOccurrence();
        public TimeSpan ElapsedTime => IsRunning
            ? DateTimeOffset.Now - (_startTime ?? DateTimeOffset.Now)
            : TimeSpan.Zero;



        public static JobTimer New<TJob>(string cronExpression, JobSchedulerConfiguration config) where TJob : IScheduledJob
        {
            return new JobTimer(typeof(TJob), cronExpression, config);
        }

        private JobTimer(Type jobType, string cronExpression, JobSchedulerConfiguration config)
        {
            _config = config;
            _cronWrapper = new CronExpressionWrapper(cronExpression, _config.IsUsingUtcTime);

            _timer = new Timer((callback) =>
            {
                var job = Activator.CreateInstance((Type)callback) as IScheduledJob;
                _cancellationToken = new JobCancellationTokenSource();
                Task.Factory.StartNew(() => ExecuteJob(job, _cancellationToken.Token),
                        _cancellationToken.Token,
                        TaskCreationOptions.LongRunning,
                        new TaskScheduler())
                    .ContinueWith(task => HandleTaskAfter(task, jobType.Name));
            }, jobType,
                (long)_cronWrapper.GetStartTime().TotalMilliseconds,
                (long)_cronWrapper.GetNextStartTime().TotalMilliseconds);
        }

        public void Trigger()
        {
            if (IsRunning || IsCancelling)
            {
                return;
            }

            _manuallyTriggered = true;
            //trigger job immediately
            _timer.Change(0, -1);
        }

        public void Cancel()
        {
            if (!IsRunning && !IsCancelling)
            {
                return;
            }

            IsCancelling = true;

            _cancellationToken?.Cancel();
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _cancellationToken?.Dispose();
        }

        private void Start()
        {
            IsCancelling = false;
            IsRunning = true;
            _startTime = DateTimeOffset.Now;
        }

        private void Stop(bool cancelled = false)
        {
            if (!cancelled)
            {
                LastExecutionDuration = ElapsedTime;
            }

            IsRunning = false;
            _startTime = null;
        }

        private void ExecuteJob(IScheduledJob job, CancellationToken token)
        {
            var name = job.GetType().Name;

            _config?.OnJobStart?.Invoke(name);

            Start();
            job.Execute(token);
            Stop();

            _config?.OnJobEnd?.Invoke(name);
        }

        private void HandleTaskAfter(Task task, string name)
        {
            if (IsRunning)
            {
                Stop(task.IsCanceled);
            }

            if (_manuallyTriggered)
            {
                //change to previous schedule
                _timer.Change((long)_cronWrapper.GetStartTime().TotalMilliseconds,
                    (long)_cronWrapper.GetNextStartTime().TotalMilliseconds);

                _manuallyTriggered = false;
            }

            var date = GetCurrentDate();
            if (!task.IsCanceled)
            {
                if (task.Exception != null)
                {
                    _config?.OnErrorCallback?.Invoke(name, task.Exception);
                }

                LastRun = date;
                RunWithSuccess = task.Exception == null;

                return;
            }

            _config?.OnJobCancelled?.Invoke(name);
            LastCancelled = date;
            RunWithSuccess = false;
            IsCancelling = false;
        }

        private DateTime GetCurrentDate()
        {
            return _config.IsUsingUtcTime ? DateTime.UtcNow : DateTime.Now;
        }
    }
}

using Element.JobScheduler.Interfaces;
using Element.Models;
using System;

namespace Element.JobScheduler
{
    public class JobSchedulerConfiguration : IDisposable
    {
        internal bool IsUsingUtcTime { get; private set; }
        public IScheduledJobStorageProvider StorageProvider { get; private set; }

        public void UseUtcTime()
        {
            IsUsingUtcTime = true;
        }

        public void UseStorageProvider(IScheduledJobStorageProvider provider)
        {
            StorageProvider = provider;
        }

        public Action<string> OnJobStart { get; set; }
        public Action<string> OnJobEnd { get; set; }
        public Action<string> OnJobCancelled { get; set; }
        public Action<string, Exception> OnErrorCallback { get; set; }

        public void Dispose()
        {
            StorageProvider?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

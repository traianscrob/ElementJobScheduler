using Element.JobScheduler.Interfaces;
using System;

namespace Element.JobScheduler
{
    public class JobSchedulerConfiguration
    {
        internal IScheduledJobStorageProvider StorageProvider { get; set; }

        public void UseStorageProvider(IScheduledJobStorageProvider provider)
        {
            StorageProvider = provider;
        }

        public Action<string> OnJobStart { get; set; }

        public Action<string> OnJobEnd { get; set; }

        public Action<Exception> OnErrorCallback { get; set; }
    }
}

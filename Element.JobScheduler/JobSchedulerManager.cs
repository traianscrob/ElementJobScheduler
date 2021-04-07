using Element.JobScheduler.Interfaces;
using System;

namespace Element.JobScheduler
{
    public static class JobSchedulerManager
    {
        public static IJobScheduler Create(Action<JobSchedulerConfiguration> config)
        {
            return new JobScheduler(config);
        }
    }
}

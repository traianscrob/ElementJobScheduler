using Element.JobScheduler.Interfaces;
using System;

namespace Element.JobScheduler
{
    public static class JobSchedulerManager
    {
        public static IJobScheduler Instance { get; private set; }

        public static void EnableJobScheduler(Action<JobSchedulerConfiguration> config) => Instance = new JobScheduler(config);
    }
}

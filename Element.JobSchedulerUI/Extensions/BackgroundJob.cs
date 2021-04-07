using Element.JobScheduler;
using Element.JobScheduler.Interfaces;
using System;

namespace Element.JobSchedulerUI.Extensions
{
    public class BackgroundJob
    {
        public static IJobScheduler Instance { get; private set; }

        internal static void EnableJobScheduler(Action<JobSchedulerConfiguration> config) => Instance = JobSchedulerManager.Create(config);
    }
}

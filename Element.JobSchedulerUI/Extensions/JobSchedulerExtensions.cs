using Element.JobScheduler;
using Element.JobScheduler.Configuration;
using Element.JobScheduler.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace Element.JobSchedulerUI.JobScheduler
{
    public static class JobSchedulerExtensions
    {
        public static void UseJobScheduler(this HttpConfiguration configuration, Action<JobSchedulerConfiguration> config)
        {
            JobSchedulerManager.EnableJobScheduler(config);
            JobSchedulerManager.Instance.EnqueueJobsFromConfig();
        }

        internal static void EnqueueJobsFromConfig(this IJobScheduler scheduler)
        {
            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => typeof(IScheduledJob).IsAssignableFrom(type))
                .ToArray();

            var jobsSection = (JobSchedulerSection)ConfigurationManager.GetSection("JobSchedulerSection");
            foreach (JobConfigElement job in jobsSection.Jobs)
            {
                var jobType = jobTypes.SingleOrDefault(x => x.Name.Equals(job.Name));
                if (jobType == null)
                {
                    continue;
                }

                var method = typeof(IJobScheduler).GetMethod(nameof(IJobScheduler.ScheduleJob));
                var generic = method?.MakeGenericMethod(jobType);
                generic?.Invoke(scheduler, new object[] { job.Schedule });
            }
        }
    }
}

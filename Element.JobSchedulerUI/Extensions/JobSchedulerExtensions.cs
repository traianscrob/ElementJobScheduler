using Element.JobScheduler;
using Element.JobSchedulerUI.Extensions;
using System;
using System.Web.Http;
using System.Web.Services.Description;

namespace Element.JobSchedulerUI.JobScheduler
{
    public static class JobSchedulerExtensions
    {
        public static void UseElementJobs(this HttpConfiguration configuration, Action<JobSchedulerConfiguration> config)
        {
            BackgroundJob.EnableJobScheduler(config);
        }

        public static void UseElementJobsDashboard(this ServiceCollection services, Action<JobSchedulerConfiguration> config)
        {
            services.Add(new Service() {

            });

            BackgroundJob.EnableJobScheduler(config);
        }
    }
}

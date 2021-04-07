using Element.JobScheduler.Configuration;
using Element.JobScheduler.Interfaces;
using Element.JobSchedulerUI.Extensions;
using Element.JobSchedulerUI.JobScheduler;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WebGrease;

namespace Element.JobSchedulerUI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var logger = new LogManager((log, msg) =>
            {
                
            },
            (warning) =>
            {

            }, null, (error) => { }, null, null);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.UseElementJobs((configuration) => {
                configuration.OnErrorCallback = (ex) => { };
                configuration.OnJobStart = (job) =>
                {
                    logger.Information($"{job} started at {DateTime.Now}");
                };
                configuration.OnJobEnd = (job) =>
                {
                    logger.Information($"{job} finished at {DateTime.Now}");
                };

                logger.Information($"JobScheduler is up and running!");
            });

            EnqueueJobsFromConfig();
        }

        private static void EnqueueJobsFromConfig()
        {
            var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
                                        .SelectMany(s => s.GetTypes())
                                        .Where(type => typeof(IScheduledJob).IsAssignableFrom(type));

            var jobsSection = (JobSchedulerSection)ConfigurationManager.GetSection("JobSchedulerSection");
            foreach (JobConfigElement job in jobsSection.Jobs)
            {
                var jobType = jobTypes.SingleOrDefault(x => x.Name.Equals(job.Name));
                if (jobType == null)
                {
                    continue;
                }

                MethodInfo method = typeof(IJobScheduler).GetMethod(nameof(IJobScheduler.ScheduleJob));
                MethodInfo generic = method.MakeGenericMethod(jobType);
                generic.Invoke(BackgroundJob.Instance, new[] { job.Schedule });
            }
        }
    }
}

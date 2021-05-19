using Element.JobScheduler;
using Element.JobScheduler.Interfaces;

namespace Element.JobSchedulerUI.Extensions
{
    public class BackgroundJob
    {
        public static IJobScheduler Instance => JobSchedulerManager.Instance;
    }
}

using Element.JobScheduler.Interfaces;
using System.Threading;

namespace Element.JobSchedulerUI.Jobs
{
    public class SomeOtherJob : IScheduledJob
    {
        public void Execute(CancellationToken token)
        {

        }
    }
}
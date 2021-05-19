using System.Threading;

namespace Element.JobScheduler.Interfaces
{
    public interface IScheduledJob
    {
        void Execute(CancellationToken token);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Element.JobScheduler.Interfaces
{
    public interface IScheduledJobStorageProvider : IDisposable
    {
        Task PersistJob(ScheduledJobStorageEntity job);
        Task PersistHistory(ScheduledJobStorageEntityHistory job);

        Task<ScheduledJobStorageEntity> Get(string name);

        List<ScheduledJobStorageEntity> GetJobs();

        List<ScheduledJobStorageEntityHistory> GetHistory(string jobName);
    }
}

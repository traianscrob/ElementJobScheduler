using Element.JobScheduler.Models;
using System;
using System.Collections.Generic;

namespace Element.JobScheduler.Interfaces
{
    public interface IJobScheduler
    {
        IJobScheduler ScheduleJob<T>(string cronExpression) where T : IScheduledJob;
        IJobScheduler Execute(Action action);
        
        void Trigger(string job);
        void Cancel(string job);
        IEnumerable<JobModel> GetJobs();
    }
}

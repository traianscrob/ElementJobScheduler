using System;

namespace Element.JobScheduler.Interfaces
{
    public interface IJobScheduler
    {
        IJobScheduler ScheduleJob<T>(string cronExpression) where T : IScheduledJob;

        IJobScheduler Execute<T>() where T : IScheduledJob;
        IJobScheduler Execute(Action action);
    }
}

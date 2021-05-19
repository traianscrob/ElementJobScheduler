using System;

namespace Element.JobScheduler
{
    public class ScheduledJobStorageEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CronExpression { get; set; }
    }
}

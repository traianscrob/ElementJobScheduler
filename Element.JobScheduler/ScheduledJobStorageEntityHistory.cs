using System;

namespace Element.JobScheduler
{
    public class ScheduledJobStorageEntityHistory
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public string Description { get; set; }

        public DateTime ExecutionDate { get; set; }

        public bool RunSuccessfuly { get; set; }
    }
}

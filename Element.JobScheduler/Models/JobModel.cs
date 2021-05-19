using System;

namespace Element.JobScheduler.Models
{
    public class JobModel
    {
        public string Name { get; set; }

        public DateTime? LastRunDate { get; set; }

        public TimeSpan? LastExecutionDuration { get; set; }

        public DateTime? LastCancelledDate { get; set; }

        public bool? RunWithSuccess { get; set; }

        public DateTimeOffset NextRunDate { get; set; }

        public bool IsRunning { get; set; }
        public bool IsCancelling { get; set; }
    }
}

using System;

namespace Element.JobScheduler
{
    public class JobSchedulerConfiguration
    {
        public Action<string> OnJobStart { get; set; }

        public Action<string> OnJobEnd { get; set; }

        public Action<Exception> OnErrorCallback { get; set; }
    }
}

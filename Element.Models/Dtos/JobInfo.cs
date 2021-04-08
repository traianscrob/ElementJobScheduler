using System;

namespace Element.Models.Dtos
{
    public class JobInfo
    {
        public string Name { get; set; }

        public string  CronExpression { get; set; }

        public DateTime? LastSuccessfulRun { get; set; }
    }
}

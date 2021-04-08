using System;

namespace Element.Models.Dtos
{
    public class JobHistoryInfo
    {
        public string JobName { get; set; }

        public string Description { get; set; }

        public bool RunSuccessfuly { get; set; }

        public DateTime ExecutionDate { get; set; }
    }
}

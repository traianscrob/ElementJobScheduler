using System;
using System.ComponentModel.DataAnnotations;

namespace Element.Models
{
    public class Job
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime LastSuccessfulRun { get; set; }
    }
}

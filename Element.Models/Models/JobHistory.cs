using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Element.Models
{
    public class JobHistory
    {
        [Key]
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public string Description { get; set; }

        public bool WasSuccessful { get; set; }

        public DateTime ExecutionDate { get; set; }

        [ForeignKey("Id")]
        public virtual ICollection<Job> Jobs { get; set; }
    }
}

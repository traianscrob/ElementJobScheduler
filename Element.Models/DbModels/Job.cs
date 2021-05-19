using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Element.Models.DbModels
{
    [Table("Jobs", Schema = "Element")]
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CronExpression { get; set; }

        public DateTime? LastSuccessfulRun { get; set; }

        public ICollection<JobHistory> JobHistory { get; set; }
    }
}

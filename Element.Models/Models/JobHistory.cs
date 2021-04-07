using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Element.Models
{
    public class JobHistory
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Id")]
        public Guid JobId { get; set; }

        public string Description { get; set; }

        public DateTime ExecutionDate { get; set; }
    }
}

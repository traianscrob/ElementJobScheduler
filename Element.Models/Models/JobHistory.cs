using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Element.Models
{
    [Table("JobHistories", Schema = "Element")]
    public class JobHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0), Key]
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public string Description { get; set; }

        public bool RunSuccessfuly { get; set; }

        public DateTime ExecutionDate { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
    }
}

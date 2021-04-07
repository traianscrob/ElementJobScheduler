using Element.Data.Interfaces;
using Element.Models;
using System.Data.Entity;

namespace Element.Data
{
    public class ElementDbContext : DbContext, IElementDbContext
    {
        public IDbSet<Job> Jobs { get; set; }
        public IDbSet<JobHistory> JobHistories { get; set; }
    }
}

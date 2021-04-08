using Element.Data.Interfaces;
using Element.Models;
using System.Data.Entity;

namespace Element.Data
{
    public class ElementDbContext : DbContext, IElementDbContext
    {
        public ElementDbContext()
            : base("name=ConnectionString")
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
        }

        public IDbSet<Job> Jobs { get; set; }
        public IDbSet<JobHistory> JobHistories { get; set; }
    }
}

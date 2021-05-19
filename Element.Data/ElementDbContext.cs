using Element.Data.Interfaces;
using Element.Models.DbModels;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace Element.Data
{
    public class ElementDbContext : DbContext, IElementDbContext
    {
        public ElementDbContext()
            : base("name=ConnectionString")
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = false;
        }

        public IDbSet<Job> Jobs { get; set; }
        public IDbSet<JobHistory> JobHistories { get; set; }
    }
}

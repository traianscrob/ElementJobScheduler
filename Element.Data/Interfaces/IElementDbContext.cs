using Element.Models.DbModels;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Element.Data.Interfaces
{
    public interface IElementDbContext : IDisposable
    {
        IDbSet<Job> Jobs { get; set; }
        IDbSet<JobHistory> JobHistories { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

        DbSet<T> Set<T>() where T : class;
    }
}

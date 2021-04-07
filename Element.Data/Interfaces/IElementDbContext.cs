using Element.Models;
using System;
using System.Data.Entity;

namespace Element.Data.Interfaces
{
    public interface IElementDbContext : IDisposable
    {
        IDbSet<Job> Jobs { get; set; }
        IDbSet<JobHistory> JobHistories { get; set; }

        int SaveChanges();
    }
}

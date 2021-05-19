using System;
using System.Threading.Tasks;

namespace Element.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IElementDbContext DbContext { get; }

        int Save();

        Task<int> SaveChangesAsync();

        IElementRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}

using Element.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Element.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(IElementDbContext dbContext)
        {
            DbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IElementDbContext DbContext { get; private set; }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public IElementRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IElementRepository<TEntity>;
            }

            var repository = new ElementRepository<TEntity>(DbContext);

            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                DbContext.Dispose();
                _disposed = true;
            }
        }
    }
}

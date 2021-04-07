using Element.Data.Interfaces;
using System;

namespace Element.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IElementDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IElementDbContext DbContext { get; private set; }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

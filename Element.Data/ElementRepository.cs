using Element.Data.Interfaces;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Element.Data
{
    public class ElementRepository<T> : IElementRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        protected IElementDbContext Context;

        public ElementRepository(IElementDbContext context)
        {
            Context = context;
            _dbSet = Context.Set<T>();
        }

        public virtual IQueryable<T> All => _dbSet;

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Merge(T entity)
        {
            _dbSet.AddOrUpdate(entity);
        }
    }
}

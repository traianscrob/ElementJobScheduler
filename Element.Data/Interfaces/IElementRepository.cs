using System.Linq;

namespace Element.Data.Interfaces
{
    public interface IElementRepository<T> where T: class
    {
        IQueryable<T> All { get; }

        void Add(T entity);
        void Merge(T entity);
        void Delete(T entity);
    }
}

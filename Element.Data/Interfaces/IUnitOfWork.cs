using System;

namespace Element.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IElementDbContext DbContext { get; }

        int Save();
    }
}

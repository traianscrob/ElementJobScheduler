using System;
using System.Collections.Generic;

namespace Element.JobScheduler.Interfaces
{
    internal interface IDisposableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
        where TValue : IDisposable
    {
    }
}

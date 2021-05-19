using Element.JobScheduler.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Element.JobScheduler
{
    internal class DisposableConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IDisposableDictionary<TKey, TValue>
        where TValue : IDisposable
    {
        public void Dispose()
        {
            foreach (var item in Values)
            {
                item?.Dispose();
            }

            Clear();
        }
    }
}

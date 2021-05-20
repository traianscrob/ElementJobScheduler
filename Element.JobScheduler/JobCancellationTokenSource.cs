using System.Threading;

namespace Element.JobScheduler
{
    public class JobCancellationTokenSource: CancellationTokenSource
    {
        public bool IsDisposed { get; set; }

        public new void Cancel()
        {
            if (IsDisposed || IsCancellationRequested)
            {
                return;
            }

            base.Cancel();
            Dispose();
        }

        public new void Dispose()
        {
            if (!IsDisposed)
            {
                base.Dispose();
            }

            IsDisposed = true;
        }
    }
}

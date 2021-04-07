using Element.JobScheduler.Interfaces;
using System;

namespace Element.Test.JobScheduler
{
    public class TestTJob : IScheduledJob
    {
        public void Execute()
        {
            Console.WriteLine("Executed job with id:" + Guid.NewGuid().ToString());
        }
    }
}
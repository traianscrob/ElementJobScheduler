﻿using Element.JobScheduler.Interfaces;
using System.Threading;

namespace Element.JobSchedulerUI.Jobs
{
    public class TestJob : IScheduledJob
    {
        public void Execute(CancellationToken token)
        {
            
        }
    }
}
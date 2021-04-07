using Element.JobScheduler;
using System;

namespace Element.Test.JobScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            JobSchedulerManager.Instance.Configure((config) => {
                
            });

            JobSchedulerManager.Instance.Execute(() => { Console.WriteLine("This one is called from the Action:" + Guid.NewGuid()); });

            Console.ReadLine();
        }
    }
}

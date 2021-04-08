using Element.Models.Dtos;

namespace Element.JobScheduler.Interfaces
{
    public interface IScheduledJobStorageProvider
    {
        /// <summary>
        /// adds or updates info related to a job
        /// </summary>
        void SaveJob(JobInfo job);

        /// <summary>
        /// adds info related to jobs
        /// </summary>
        void AddHistory(JobHistoryInfo history);

        /// <summary>
        /// deletes a job
        /// </summary>
        /// <param name="name"></param>
        void Delete(string name);
    }
}

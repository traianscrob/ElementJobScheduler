using Element.Data.Interfaces;
using Element.JobScheduler.Interfaces;
using Element.Models;
using Element.Models.Dtos;
using System;
using System.Linq;

namespace Element.JobSchedulerUI.Extensions
{
    public class SqlStorageProvider : IScheduledJobStorageProvider
    {
        private readonly IUnitOfWork _unitOfWork;

        public SqlStorageProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SaveJob(JobInfo job)
        {
            var entity = _unitOfWork.DbContext.Jobs.SingleOrDefault(x => x.Name.Equals(job.Name)) ?? _unitOfWork.DbContext.Jobs.Create();
            if (!string.IsNullOrEmpty(entity.Name))
            {
                entity.LastSuccessfulRun = DateTime.Now;
                entity.CronExpression = job.CronExpression;
                entity.LastSuccessfulRun = job.LastSuccessfulRun;
            }
            else
            {
                entity.Name = job.Name;
                entity.CronExpression = job.CronExpression;

                _unitOfWork.DbContext.Jobs.Add(entity);
            }

            _unitOfWork.Save();
        }

        public void AddHistory(JobHistoryInfo history)
        {
            var job = _unitOfWork.DbContext.Jobs.SingleOrDefault(x => x.Name.Equals(history.JobName));
            if (job != null)
            {
                _unitOfWork.DbContext.JobHistories.Add(new JobHistory()
                {
                    JobId = job.Id,
                    RunSuccessfuly = history.RunSuccessfuly,
                    ExecutionDate = history.ExecutionDate,
                    Description = history.Description
                });

                _unitOfWork.Save();
            }
        }

        public void Delete(string name)
        {
            var entity = _unitOfWork.DbContext.Jobs.SingleOrDefault(x => x.Name.Equals(name));
            if (entity != null)
            {
                _unitOfWork.DbContext.Jobs.Remove(entity);
                _unitOfWork.Save();
            }
        }
    }
}
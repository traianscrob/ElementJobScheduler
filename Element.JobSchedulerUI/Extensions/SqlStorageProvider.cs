using Element.Data.Interfaces;
using Element.JobScheduler;
using Element.JobScheduler.Interfaces;
using Element.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Element.JobSchedulerUI.Extensions
{
    public class SqlStorageProvider : IScheduledJobStorageProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MemoryCache _cache;

        public SqlStorageProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _cache = new MemoryCache(typeof(SqlStorageProvider).Name);
        }

        public async Task<ScheduledJobStorageEntity> Get(string name)
        {
            return _unitOfWork.DbContext.Jobs
                .Select(j => new ScheduledJobStorageEntity
                {
                    Id = j.Id,
                    Name = j.Name,
                    CronExpression = j.CronExpression,
                }).SingleOrDefault(x => x.Name.Equals(name));
        }

        public async Task PersistJob(ScheduledJobStorageEntity job)
        {
            var entity = _unitOfWork.DbContext.Jobs.SingleOrDefault(x => x.Name == job.Name);
            if (entity != null)
            {
                entity.LastSuccessfulRun = DateTime.Now;
                entity.CronExpression = job.CronExpression;
            }
            else
            {
                entity = new Job()
                {
                    Name = job.Name,
                    CronExpression = job.CronExpression,
                };

                _unitOfWork.DbContext.Jobs.Add(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task PersistHistory(ScheduledJobStorageEntityHistory job)
        {
            var entity = _unitOfWork.DbContext.JobHistories.Add(new JobHistory {
                JobId = job.EntityId,
                Description = job.Description,
                ExecutionDate = job.ExecutionDate,
                RunSuccessfuly = job.RunSuccessfuly
            });

            await _unitOfWork.DbContext.SaveChangesAsync();
        }

        public void RemoveJob(string name)
        {
            var entity = _unitOfWork.DbContext.Jobs.SingleOrDefault(x => x.Name.Equals(name));
            if (entity != null)
            {
                _unitOfWork.DbContext.Jobs.Remove(entity);
                _unitOfWork.DbContext.SaveChanges();
            }
        }

        public List<ScheduledJobStorageEntity> GetJobs()
        {
            return _unitOfWork.DbContext.Jobs.Select(j => new ScheduledJobStorageEntity
            {
                Id = j.Id,
                Name = j.Name,
                CronExpression = j.CronExpression
            }).ToList();
        }

        public List<ScheduledJobStorageEntityHistory> GetHistory(string jobName)
        {
            return _unitOfWork.DbContext.Jobs
                .Join(_unitOfWork.DbContext.JobHistories, j => j.Id, h => h.JobId, (j, h) => new { j.Name, h })
                .Where(x => x.Name.Equals(jobName))
                .Select(x => x.h)
                .Select(j => new ScheduledJobStorageEntityHistory
                {
                    Id = j.Id,
                    EntityId = j.JobId,
                    Description = j.Description,
                    ExecutionDate = j.ExecutionDate,
                    RunSuccessfuly = j.RunSuccessfuly
                }).ToList();
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
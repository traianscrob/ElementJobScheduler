using Cronos;
using System;

namespace Element.JobScheduler
{
    internal class CronExpressionWrapper
    {
        private readonly CronExpression _cronExpression;
        private readonly bool _useUtc;

        public CronExpressionWrapper(string cronExp, bool useUtc = false)
        {
            _useUtc = useUtc;
            _cronExpression = CronExpression.Parse(cronExp, CronFormat.Standard);
        }

        public DateTimeOffset GetNextOccurrence()
        {
            var currentDate = _useUtc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            var timeZone = _useUtc ? TimeZoneInfo.Utc : TimeZoneInfo.Local;

            return _cronExpression.GetNextOccurrence(currentDate, timeZone) ?? currentDate;
        }

        public DateTimeOffset GetNextOccurrence(DateTimeOffset date)
        {
            var timeZone = _useUtc ? TimeZoneInfo.Utc : TimeZoneInfo.Local;

            return _cronExpression.GetNextOccurrence(date, timeZone) ?? date;
        }

        public TimeSpan GetStartTime()
        {
            var currentDate = _useUtc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;

            return GetNextOccurrence() - currentDate;
        }

        public TimeSpan GetNextStartTime()
        {
            var nextDate = GetNextOccurrence();
            var nextNextDate = GetNextOccurrence(nextDate);

            return nextNextDate - nextDate;
        }
    }
}

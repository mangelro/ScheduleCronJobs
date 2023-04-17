


using Cronos;

namespace ScheduleCronJobs.Services
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        bool IncludeSeconds { get; set; }
    }


    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; } = "";
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Utc;
        public bool IncludeSeconds { get; set; } = false;
    }
}

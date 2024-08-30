namespace ScheduleCronJobs.Services
{
    public interface IScheduleConfig<in T>
        where T : CronJobService
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        bool IncludeSeconds { get; }
    }



    public class ScheduleConfig<T> : IScheduleConfig<T>
        where T : CronJobService
    {


        public string CronExpression { get; set; } = "";
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Utc;
        public bool IncludeSeconds => CronExpression.Split(' ').Length == 6;
    }
}

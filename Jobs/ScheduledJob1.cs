using ScheduleCronJobs.Services;

namespace ScheduleCronJobs.Jobs
{
    public class ScheduledJob1 : CronJobService
    {

        private readonly ILogger<ScheduledJob1> _logger;

        public ScheduledJob1(IScheduleConfig<ScheduledJob1> config, ILogger<ScheduledJob1> logger) : base(config.CronExpression, config.TimeZoneInfo,config.IncludeSeconds)
        {
            _logger=logger??throw new ArgumentNullException(nameof(logger));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{type} start", this.GetType().Name);
            return base.StartAsync(cancellationToken);
        }

        protected override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{time} : {type} work",DateTime.Now.ToString("T"), this.GetType().Name);
            await Task.Delay(TimeSpan.FromSeconds(1),cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{type} stop", this.GetType().Name);
            return base.StopAsync(cancellationToken);
        }

    }
}

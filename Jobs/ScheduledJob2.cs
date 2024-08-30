using ScheduleCronJobs.Services;

namespace ScheduleCronJobs.Jobs
{
    public class ScheduledJob2 : CronJobService
    {

        private readonly ILogger<ScheduledJob2> _logger;

        public ScheduledJob2(IScheduleConfig<ScheduledJob2> config, ILogger<ScheduledJob2> logger) : base(config.CronExpression, config.TimeZoneInfo,config.IncludeSeconds, logger)
        {
            _logger=logger??throw new ArgumentNullException(nameof(logger));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{type} start 2", this.GetType().Name);
            return base.StartAsync(cancellationToken);
        }

        protected override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{time} : {type} work 2 start", DateTime.Now.ToString("T"), this.GetType().Name);

            Random random = new Random();

            TimeSpan duracion = TimeSpan.FromSeconds(random.Next(20));

            await Task.Delay(duracion, cancellationToken);

            _logger.LogInformation("{time} : {type} work 2 end {duracion}", DateTime.Now.ToString("T"), this.GetType().Name, duracion);


        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{type} stop 2", this.GetType().Name);
            return base.StopAsync(cancellationToken);
        }

    }
}

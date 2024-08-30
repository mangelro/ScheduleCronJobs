using Cronos;

namespace ScheduleCronJobs.Services
{
    public abstract class CronJobService : BackgroundService
    {
        private System.Timers.Timer? _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly bool _includeSeconds;
        private readonly ILogger _logger;

        protected CronJobService(string expression, TimeZoneInfo timeZoneInfo, bool includeSeconds, ILogger logger)
        {
            _expression = CronExpression.Parse(expression ?? throw new ArgumentNullException(nameof(expression)), includeSeconds ? CronFormat.IncludeSeconds : CronFormat.Standard);
            _timeZoneInfo = timeZoneInfo ?? throw new ArgumentNullException(nameof(timeZoneInfo));
            _includeSeconds = includeSeconds;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return ScheduleJob(stoppingToken);
        }


#if NET6_0_OR_GREATER
        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            DateTimeOffset? next = _expression.GetNextOccurrence(DateTimeOffset.UtcNow, _timeZoneInfo);

            while (next.HasValue && !cancellationToken.IsCancellationRequested)
            {
                var delay = next.Value - DateTimeOffset.UtcNow;

                _logger.LogInformation("Next work {nexttxt} [{delaytxt}]",next.Value.ToString("T"), delay.ToString(@"hh\:mm\:ss"));

                //TODO: Si delay.TotalMiliseconds < 0


                using PeriodicTimer timer = new PeriodicTimer(delay);


                if (await timer.WaitForNextTickAsync(cancellationToken))
                    await DoWork(cancellationToken);

                next = _expression.GetNextOccurrence(DateTimeOffset.UtcNow, _timeZoneInfo);
            }

        }




#else



        private async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.UtcNow, _timeZoneInfo, _includeSeconds);

            if (next.HasValue && !cancellationToken.IsCancellationRequested)
            {

                var delay = next.Value - DateTimeOffset.UtcNow;

                if (delay.TotalMilliseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);
                    return;
                }

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);

                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);    // reschedule next
                    }
                };
                _timer.Start();
            }

            await Task.CompletedTask;
        }
#endif



        protected abstract Task DoWork(CancellationToken cancellationToken);


        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }





    }






}

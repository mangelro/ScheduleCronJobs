

using Cronos;

namespace ScheduleCronJobs.Services
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer? _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly bool _includeSeconds;


        protected CronJobService(string expression, TimeZoneInfo timeZoneInfo, bool includeSeconds)
        {
            _expression = CronExpression.Parse(expression ?? throw new ArgumentNullException(nameof(expression)), includeSeconds?CronFormat.IncludeSeconds:CronFormat.Standard);
            _timeZoneInfo = timeZoneInfo ?? throw new ArgumentNullException(nameof(timeZoneInfo)); ;
            _includeSeconds= includeSeconds;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            return ScheduleJob(cancellationToken);
        }


        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo,_includeSeconds);

            if (next.HasValue)
            {

                var delay = next.Value - DateTimeOffset.Now;

                if (delay.TotalMilliseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);
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

        protected abstract Task DoWork(CancellationToken cancellationToken);

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }


    }






}

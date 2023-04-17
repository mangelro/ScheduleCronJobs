


using ScheduleCronJobs.Services;

namespace ScheduleCronJobs.Configuration
{
    public static class CronJobServiceExtensions
    {

        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options ) 
            where T : CronJobService
        {

            if (options is null ) 
            {
            throw new ArgumentNullException(nameof(options));
            }

            var config=new ScheduleConfig<T>();

            options.Invoke(config);

            services.AddSingleton<IScheduleConfig<T>>(config);

            services.AddHostedService<T>();

            return services;

        }


    }
}

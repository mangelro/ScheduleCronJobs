using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

using ScheduleCronJobs.Configuration;
using ScheduleCronJobs.Jobs;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();


try
{



    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    //builder.Services.AddCronJob<ScheduledJob1>(c =>
    //{
    //    c.IncludeSeconds = false;
    //    c.TimeZoneInfo = TimeZoneInfo.Local;
    //    c.CronExpression = "0 7 * * *";

    //  //  7:80 UTC => 9:40 local
    //});


    builder.Services.AddCronJob<ScheduledJob2>(c =>
    {
        c.TimeZoneInfo = TimeZoneInfo.Local;
       
        c.CronExpression = "*/15 * * * * *"; //cada 15 segundos 
    });



    builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console());


    //builder.Logging.ClearProviders();
    //builder.Logging.AddConsole();


    var app = builder.Build();

     
    //app.UseSerilogRequestLogging(); // <-- Add this line

    // Configure the HTTP request pipeline.


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
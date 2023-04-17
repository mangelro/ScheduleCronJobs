using ScheduleCronJobs.Configuration;
using ScheduleCronJobs.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCronJob<ScheduledJob1>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Local;
    c.IncludeSeconds = true;    
    c.CronExpression =  @"*/30 * * * * *"; //cada 30 segundos 
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

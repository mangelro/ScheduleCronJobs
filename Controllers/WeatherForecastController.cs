using Cronos;

using Microsoft.AspNetCore.Mvc;

namespace ScheduleCronJobs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet("cron")]
        public IActionResult GetCron(string ex = "* * * * *")
        {

            var includeSeconds = ex.Split(' ').Length == 6;

            CronExpression expression = CronExpression.Parse(ex, includeSeconds ? CronFormat.IncludeSeconds : CronFormat.Standard);

            DateTime ahora = DateTime.UtcNow;

            IEnumerable<DateTime> occurrences = expression.GetOccurrences(
                ahora,
                ahora.AddDays(1),
                TimeZoneInfo.Local,
                fromInclusive: false,
                toInclusive: false);


            return Ok(new {Expresion=ex,Ahora= ahora.ToString("s"), occurrences });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using communication.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace communication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(DataContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        // public WeatherForecastController(ILogger<WeatherForecastController> logger)
        // {
        //     _logger = logger;
        // }

        [HttpGet]
        //IEnumerable<WeatherForecast>
        public IActionResult GetWeatherForecast()
        {
            // var rng = new Random();
            // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            // {
            //     Date = DateTime.Now.AddDays(index),
            //     TemperatureC = rng.Next(-20, 55),
            //     Summary = Summaries[rng.Next(Summaries.Length)]
            // })

            // .ToArray();
            var result = _context.Values.ToList();
            return Ok(result);

        }

        [HttpGet("{id}")]
        public ActionResult<int> GetGetWeatherForecast(int id)
        {
            var value = _context.Values.FirstOrDefault(w => w.Id == id);
            // throw new Exception("Fire Exception!!!");
            return Ok(value);
        }

    }
}

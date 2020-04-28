using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MetaWeather.Controllers;

namespace MetaWeather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static async Task GetWeatherForecastForInputAsync(MetaWeatherController metaWeatherController, Task<List<LocationSearchResult>> locations)
        {
            await metaWeatherController.Locationforecast(locations.Result[0].WOEID);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

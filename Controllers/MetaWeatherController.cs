using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MetaWeather.Models;
using MetaWeather.Context;

namespace MetaWeather.Controllers
{
    [ApiController]
    [Route("api/")]
    public class MetaWeatherController : ControllerBase
    {
        readonly LocationContext _context;
        public MetaWeatherController(LocationContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("search")]
        public async Task<List<LocationSearchResult>> SearchForLocationsAsync(string query)
        {
            return await LocationSearchApi(query);
        }


        [HttpGet]
        [Route("forecast")]
        public async Task<LocationForecast> Locationforecast(int id)
        {
            return await LocationWeatherForecastApi(id);
        }

        private async Task<List<LocationSearchResult>> LocationSearchApi(string query)
        {
            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync($"https://www.metaweather.com/api/location/search?query={query}");
                var locations = await JsonSerializer.DeserializeAsync<List<LocationSearchResult>>(stream);
                if (locations.Count == 0)
                {
                    System.Console.WriteLine("Location not found");
                    return null;
                }
                else
                {
                    var locationsToBeAdded = locations.Select(u => u.WOEID).Distinct().ToArray();
                    var locationsInDB = _context.locations.Where(u => locationsToBeAdded.Contains(u.WOEID))
                                                .Select(u => u.WOEID).ToArray();
                    var locationsNotInDb = locations.Where(u => !locationsInDB.Contains(u.WOEID));

                    foreach (LocationSearchResult location in locationsNotInDb)
                    {
                        _context.Add(location);
                        _context.SaveChanges();
                    }
                }

                foreach (LocationSearchResult location in locations)
                {
                    var entity = _context.locations.FirstOrDefault(item => item.WOEID == location.WOEID);

                    if (entity != null)
                    {
                        entity.SearchCount += 1;
                        _context.SaveChanges();
                        location.SearchCount = entity.SearchCount;
                    }
                }
                return locations;
            }
        }

        private async Task<LocationForecast> LocationWeatherForecastApi(int id)
        {
            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync($"https://www.metaweather.com/api/location/{id}");
                var forecast = await JsonSerializer.DeserializeAsync<LocationForecast>(stream);

                foreach (ConsolidatedWeather consolidatedWeather in forecast.consolidatedWeather)
                {
                    consolidatedWeather.WeatherWarning = IssueWeatherWarning(consolidatedWeather.WindSpeed);
                }
                return forecast;
            }
        }

        public string IssueWeatherWarning(double windSpeed)
        {   
            var weatherWarning = new WeatherWarning();
            switch(windSpeed)
            {
            case var d when d >= 50 && d < 60:
                weatherWarning.status = "Amber";
                break;

            case var d when d >= 60 && d < 70:
                weatherWarning.status = "Yellow";
                break;

            case var d when d >= 70:
                weatherWarning.status = "Red";
                break;

            default:
                weatherWarning.status = "No Weather Warning";
                break;
            }
            return weatherWarning.status;
        }
    }
}    


using System.Collections.Generic;
using System.Threading.Tasks;
using MetaWeather.Models;

namespace MetaWeather.Services
{
    public interface IWeatherApi 
    {
        Task<List<LocationSearchResult>> LocationSearchApi(string location);
        Task<LocationForecast> LocationWeatherForecastApi(int locationId);
        
    }
}
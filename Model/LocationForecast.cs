using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MetaWeather.Models
{
public class LocationForecast
{
    [JsonPropertyName("consolidated_weather")]
    public List<ConsolidatedWeather> consolidatedWeather {
        get;
        set;
    }
    
    [JsonPropertyName("title")]
    public string Title { get; set;}

    public string time {get; set;}

}
}
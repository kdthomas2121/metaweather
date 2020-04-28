using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MetaWeather.Models
{

public class ConsolidatedWeather
{
    [JsonPropertyName("wind_speed")]
    public double WindSpeed { get; set; } 

    [JsonPropertyName("min_temp")]
    public double MinTemp { get; set; }

    [JsonPropertyName("max_temp")]
    public double MaxTemp { get; set; }

    [JsonPropertyName("id")]
    public long ID { get; set; }

    public string WeatherWarning { get; set;}
}
}
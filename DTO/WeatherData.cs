using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Weather.DTO;

public record WeatherData(
    [property: JsonPropertyName("list")] IReadOnlyList<List> List
);

public record Metrics(
[property: JsonPropertyName("temp")] double Temp,
[property: JsonPropertyName("temp_min")] double Temp_min,
[property: JsonPropertyName("temp_max")] double Temp_max,
[property: JsonPropertyName("pressure")] int Pressure,
[property: JsonPropertyName("humidity")] int Humidity
);

public record List(
[property: JsonPropertyName("main")] Metrics Metrics,
[property: JsonPropertyName("weather")] IReadOnlyList<Weather> TextDescriptions,
[property: JsonPropertyName("dt_txt")] string EntryDate
)
{
    public DateTime EntryDateObject { get; init; } = DateTime.Parse(EntryDate);
};

public record Weather(
[property: JsonPropertyName("main")] string Forecast,
[property: JsonPropertyName("description")] string ForecastDetails
);


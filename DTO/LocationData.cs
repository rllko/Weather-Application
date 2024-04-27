using System.Text.Json.Serialization;

namespace Weather.DTO;

internal record LocationData(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("state")] string State)
{
    public override string ToString()
    {
        return $"{Name} ({State} {Country})";
    }
}


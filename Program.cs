using System.Text.Json;
using System.Text.RegularExpressions;
using Spectre.Console;
using Weather;
using Weather.displays;
using Weather.DTO;

internal class Program
{
    private static async Task Main()
    {
        // Set the output encoding of the console to enable emojis
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        AnsiConsole.Markup("Rikko/Ricardo, 2024\n[link]https://openweathermap.org[/] for the weather data\n");
        var location = AnsiConsole.Ask<string>("What's your [green]city name, state code or country code[/]?\n");

        // Dont forget to remove the api key later.
        const string ApiKey = "PUT YOUR API KEY HERE";

        var client = new HttpClient();
        
        // Fetch found locations from the API
        string url = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&appid={ApiKey}";
        string response = await client.GetStringAsync(url);

        // Serialize obtained locations
        LocationData[]? availableLocations =
        JsonSerializer.Deserialize<LocationData[]>(response) ?? throw new ArgumentException(message: "Locations object cannot be null");

        // Convert obtained locations to string
        var choices = (from loc in availableLocations select loc.ToString()).ToArray();
        
        // Check for valid length
        if(choices.Length == 0 )
        {
            Console.WriteLine("An Error occured, no such city exists!");
            Environment.Exit(1);
        }

        var selectionPrompt =  new SelectionPrompt<string>()
            .Title("Select the correct option:")
            .PageSize(5)
            .AddChoices(choices);

        // Ask the user to pick the correct one
        var obtainedLocations = AnsiConsole.Prompt(selectionPrompt);

        // Get selected location object
        LocationData selectedItem = Array.Find(availableLocations,location => location.ToString() == obtainedLocations) 
            ?? throw new ArgumentException(message: "LocationData cannot be null"); ;

        WeatherDataSource weatherDataSource = new();
        ForecastDisplay forecastDisplay = new(weatherDataSource);

        await weatherDataSource.FetchAndUpdateMeasurements(selectedItem.Lat, selectedItem.Lon, ApiKey);
        forecastDisplay.Display();
    }
}
using System.Text.Json;
using Spectre.Console;
using Weather;
using Weather.displays;
using Weather.DTO;

internal class Program
{
    private static async Task Main()
    {
        // set the output encoding of the console to enable emojis
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        AnsiConsole.Markup("Rikko/Ricardo, 2024\n[link]https://openweathermap.org[/] for the weather data\n");
        var location = AnsiConsole.Ask<string>("What's your [green]city name, state code or country code[/]?\n");

        // dont forget to remove the api key later.
        const string ApiKey = "PUT YOUR API KEY HERE";

        var client = new HttpClient();
        
        // Fetch found locations from the API
        string url = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&appid={ApiKey}";
        string response = await client.GetStringAsync(url);

        // Serialize obtained locations
        LocationData[]? availableLocations =
        JsonSerializer.Deserialize<LocationData[]>(response) ?? throw new ArgumentException(message: "Locations object cannot be null");

         var selectionPrompt =  new SelectionPrompt<string>()
            .Title("Select the correct option:")
            .PageSize(5);

        // Convert obtained locations to string
        selectionPrompt.AddChoices(GetAvailableLocations(availableLocations));

        // Ask the user to pick the correct one
        var obtainedLocations = AnsiConsole.Prompt(selectionPrompt);

        // Get selected location object
        LocationData selectedItem = availableLocations.SingleOrDefault(location => location.ToString() == obtainedLocations) ?? throw new ArgumentException(message: "LocationData cannot be null"); ;

        WeatherDataSource weatherDataSource = new();
        ForecastDisplay forecastDisplay = new(weatherDataSource);

        await weatherDataSource.FetchAndUpdateMeasurements(selectedItem.Lat, selectedItem.Lon, ApiKey);
        forecastDisplay.Display();
    }

    static string [] GetAvailableLocations(LocationData []? locations)
    {
        if(locations == null || locations.Length == 0)
        {
            Console.WriteLine("No data was found!");
            Environment.Exit(1);
        }

        // gets all the locations and set
        List<string> countryNames = [];
        foreach(LocationData? location in locations)
        {
            countryNames.Add(location.ToString());
        }

        return [.. countryNames];
    }
}
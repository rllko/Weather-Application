using System.Text.Json;
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

        // Dont forget to remove the api key later.
        const string ApiKey = "API KEY";

        // Create Start display to get input from the user
        StartDisplay startDisplay = new StartDisplay();
        startDisplay.Display();

        // Get data from the user
        LocationData selectedItem = await startDisplay.requestData(ApiKey);

        // Create Source to get data from the API
        WeatherDataSource weatherDataSource = new();

        // Create a Display to show the data
        ForecastDisplay forecastDisplay = new(weatherDataSource);

        // Fetch the data from the API and send to Observers 
        await weatherDataSource.FetchAndUpdateMeasurements(selectedItem.Lat, selectedItem.Lon, ApiKey);

        // Display data
        forecastDisplay.Display();
    }
}
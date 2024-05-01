using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Weather.DTO;
using Weather.interfaces;

namespace Weather.displays
{
    internal class StartDisplay : IDisplayElement
    {
        public void Display()
        {
            AnsiConsole.Markup($"Rikko/Ricardo, 2024{Environment.NewLine}[link]https://openweathermap.org[/] for the weather data{Environment.NewLine}");
        }
        public async Task<LocationData> requestData(string ApiKey)
        {
            var location = AnsiConsole.Ask<string>("What's your [green]city name, state code or country code[/]?\n");

            using var client = new HttpClient();

            // Fetch found locations from the API
            string url = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&appid={ApiKey}";
            string response = await client.GetStringAsync(url);

            // Serialize obtained locations
            LocationData[]? availableLocations =
                JsonSerializer.Deserialize<LocationData[]>(response) ?? throw new ArgumentException(message: "Locations object cannot be null");

            // Convert obtained locations to string
            var choices = (from loc in availableLocations select loc.ToString()).ToArray();

            // Check for valid length
            if(!choices.Any())
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
            return Array.Find(availableLocations,location => location.ToString() == obtainedLocations)
                ?? throw new ArgumentException(message: "LocationData cannot be null");
        }
    }
}

using Spectre.Console;
using System.Text.Json;
using System.Xml.Linq;
using Weather.DTO;
using Weather.interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Weather.displays
{
    internal class ForecastDisplay : IDisplayElement, IObserver
    {
        private WeatherData? weather;
        //private readonly ISubject subject;
        private Table? Table { get; set; }

        // added the subject as a constructor in case i want to update the location later
        public ForecastDisplay(ISubject subject)
        {
            subject.RegisterObserver(this);
        }

        public void Display()
        {
            // Create a table
            Table = new Table().Centered();

            // Filter the object list and group by hour, the midnight value seems to bug sometimes
            var days = weather.List.Where(item => item.EntryDateObject.Hour > 0).GroupBy(item => (item.EntryDateObject - DateTime.Today).Days);
            days.ToList().ForEach(filteredDay =>
            {
                filteredDay.ToList().ForEach(data =>
                {
                    Table.Title(data.EntryDateObject.DayOfWeek.ToString());
                    Table.AddColumn($"{data.EntryDateObject.TimeOfDay}");
                });

                List<string> temperatures = [];
                filteredDay.ToList().ForEach(data =>
                {
                    var textDescription = data.TextDescriptions[0];
                    string emoji = GetEmoji(data.TextDescriptions[0].Forecast);

                    string output = $"""
                    {textDescription.Forecast}{emoji}
                    {textDescription.ForecastDetails}
                    {data.Metrics.Temp}C
                    """;

                    temperatures.Add(output);
                });
                Table.AddRow(temperatures.ToArray()).Centered();

                // Render the table to the console
                AnsiConsole.Write(Table);
                Table = new Table().Centered();
            });
        }

        public void Update(string jsonData)
        {
            // Deserialize forecast object
            weather = JsonSerializer.Deserialize<WeatherData>(jsonData) ?? throw new ArgumentException(message: "weather cant be null"); ;
            Display();
        }

        public static string GetEmoji(string input) => input switch
        {
            "Clouds" => ":sun_behind_cloud:",
            "Rain" => ":cloud_with_rain:",
            "Clear" => ":sun:",
            _ => throw new ArgumentException(message: "unrecognized weather"),
        };
    }
}

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
        private WeatherData WeatherData;
        private readonly WeatherDataSource WeatherDataSource;
        //private readonly ISubject subject;
        private Table? Table { get; set; }

        // added the subject as a constructor in case i want to update the location later
        public ForecastDisplay(WeatherDataSource subject)
        {
            this.WeatherDataSource = subject;
            subject.RegisterObserver(this);
        }

        public void Display()
        {
            List<Table> tables = [];
            // Create a table
            Table = new Table().Centered();

            // Filter the object list and group by hour, the midnight value seems to bug sometimes
            var entries = (from day in WeatherData.List
                           select day)
                           .GroupBy(day => day.EntryDateObject.Day)
                           .Distinct()
                           .ToList();

            entries.Distinct().ToList().ForEach(filteredDay =>
            {
                List<string> temperatures = [];
                filteredDay.ToList().ForEach(data =>
                {
                    Table.Title(data.EntryDateObject.DayOfWeek.ToString());
                    Table.AddColumn($"[bold]{data.EntryDateObject.TimeOfDay}[/]").Width(100);

                    var textDescription = data.TextDescriptions[0];
                    string emoji = GetEmoji(data.TextDescriptions[0].Forecast);

                temperatures.Add($"""
                    {textDescription.Forecast} {emoji}{Environment.NewLine}
                    {textDescription.ForecastDetails}
                    {data.Metrics.Temp}C
                    """);
                });
                Table.AddRow(temperatures.ToArray()).Centered();

                tables.Add(Table);
                Table = new Table().Centered();
            });
            // Render the table to the console
            tables.ForEach(table => AnsiConsole.Write(table));

            Console.ReadKey();
            Environment.Exit(0);
        }

        public void Update()
        {
            // Deserialize forecast object
            string json = WeatherDataSource.JsonData;
            WeatherData = JsonSerializer.Deserialize<WeatherData>(json) ?? throw new ArgumentException(message: "weather cant be null"); ;
            Display();
        }

        public static string GetEmoji(string input) => input switch
        {
            "Clouds" => ":sun_behind_cloud:",
            "Rain" => ":cloud_with_lightning_and_rain:",
            "Clear" => ":sun:",
            _ => throw new ArgumentException(message: "unrecognized weather"),
        };
    }
}

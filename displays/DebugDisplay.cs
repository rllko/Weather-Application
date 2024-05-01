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
    internal class DebugDisplay : IDisplayElement, IObserver
    {
        private readonly WeatherDataSource DataSource;
        private WeatherData? WeatherData { get; set; }
        public DebugDisplay(WeatherDataSource subject)
        {
            WeatherData = null;
            subject.RegisterObserver(this);
            DataSource = subject;
        }
        public void Display()
        {
            var data = (from dat in WeatherData.List select new
            {
                dat.EntryDateObject.Date.Day,
                dat.Metrics.Temp,
                Descriptions = dat.TextDescriptions.ToString()
            }).Distinct();
            foreach (var item in data.Distinct())
            {
                Console.WriteLine(item);
            }
        }

        public void Update()
        {
            WeatherData = JsonSerializer.Deserialize<WeatherData>(DataSource.JsonData);
            Display();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.interfaces;
using Weather.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Spectre.Console;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Weather
{
    internal class WeatherDataSource : ISubject
    {
        readonly List<IObserver> observers;
        WeatherData? WeatherData { get; set; }
        private string data;

        public double GetCurrentTemperature()
        {
            if(WeatherData == null)
            {
                throw new ArgumentException(message: "Trying to get data when the object is null");
            }

            return WeatherData.List [0].Metrics.Temp;
        }

        public WeatherDataSource()
        {
            data = "";
            observers = [];
        }

        public void NotifyObservers(IEnumerable<IObserver> observers)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update(data);
            }
        }

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void MeasurementsChanged()
        {
            NotifyObservers(observers);
        }

        public async Task FetchAndUpdateMeasurements(double lat,double lon,string ApiKey)
        {
            using(var client = new HttpClient())
            {
                // Get location forecast
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&units=metric&appid={ApiKey}&cnt=17";
                data = await client.GetStringAsync(weatherUrl);
                WeatherData = JsonSerializer.Deserialize<WeatherData>(data);
            }
            MeasurementsChanged();
        }
    }
}

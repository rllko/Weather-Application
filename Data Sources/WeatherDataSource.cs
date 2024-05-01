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
        private WeatherData? WeatherData { get; set; }
        private string Data { get; set; }
        public string JsonData => Data;

        public WeatherData? weatherData => WeatherData;

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
            Data = "";
            observers = [];
        }

        public void NotifyObservers(IEnumerable<IObserver> observers)
        {
            foreach (IObserver observer in observers)
            {
                observer.Update();
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
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&units=metric&appid={ApiKey}";
                Data = await client.GetStringAsync(weatherUrl);
                WeatherData = JsonSerializer.Deserialize<WeatherData>(Data);
            }
            MeasurementsChanged();
        }
    }
}

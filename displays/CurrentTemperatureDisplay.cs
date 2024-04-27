using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.interfaces;

namespace Weather.displays
{
    internal class CurrentTemperatureDisplay : IObserver, IDisplayElement
    {
        double currentWeather;
        readonly WeatherDataSource WeatherDataSource;
        public CurrentTemperatureDisplay(WeatherDataSource WeatherDataSource) { 
            this.WeatherDataSource = WeatherDataSource;
            WeatherDataSource.RegisterObserver(this);
            currentWeather = 0;
        }
        public void Display()
        {
            Console.WriteLine($"Current Weather: {currentWeather}");
        }

        public void Update()
        {
            currentWeather = WeatherDataSource.GetCurrentTemperature();
        }
    }
}

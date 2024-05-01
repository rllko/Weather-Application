using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.interfaces;

namespace Weather.displays
{
    internal class ThirdPartyDisplay : IDisplayElement, IObserver
    {
        readonly WeatherDataSource DataSource;
        public ThirdPartyDisplay(WeatherDataSource subject) {
            subject.RegisterObserver(this);
            DataSource = subject;
        }

        public void Display()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}

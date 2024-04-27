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
        public ThirdPartyDisplay(ISubject subject) {
            subject.RegisterObserver(this);
        }

        public void Display()
        {
            throw new NotImplementedException();
        }

        public void Update(string weatherDataJson)
        {
            throw new NotImplementedException();
        }
    }
}

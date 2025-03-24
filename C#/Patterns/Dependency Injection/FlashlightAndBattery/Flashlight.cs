using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashlightAndBattery
{
    internal class Flashlight
    {
        private IBattery _battery;

        public Flashlight(IBattery battery) 
        {
            _battery = battery;
        }

        public void CheckBattery()
        {
            Console.WriteLine($"Battery brand: {_battery.GetBrand()}");
        }

        public void Charge()
        {
            Console.WriteLine($"This will take {_battery.GetChargeTime()} hours to charge");
        }
    }
}

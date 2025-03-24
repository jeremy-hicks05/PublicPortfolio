using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashlightAndBattery
{
    internal class Battery : IBattery
    {
        public string Brand;

        public int HoursToCharge;

        public void Power()
        {
            Console.WriteLine("Powering Flashlight!");
        }

        public string GetBrand()
        {
            return Brand;
        }

        public int GetChargeTime() 
        {
            return HoursToCharge;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashlightAndBattery
{
    internal interface IBattery
    {
        public void Power();
        public string GetBrand();
        public int GetChargeTime();
    }
}

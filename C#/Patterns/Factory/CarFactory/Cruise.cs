using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    internal class Cruise : Car
    {
        public Cruise() 
        {
            Make = "Chevy";
        }

        public override void Drive()
        {
            Console.WriteLine("Driving a Cruise");
        }
    }
}

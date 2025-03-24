using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    internal class Mustang : Car
    {
        public Mustang()
        {
            Make = "Ford";
        }

        public override void Drive()
        {
            Console.WriteLine("Driving a Mustang");
        }
    }
}

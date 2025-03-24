using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarFactory
{
    internal class CarFactory
    {
        public CarFactory() { }

        public Car GetCarType(string carType)
        {
            switch (carType)
            {
                case "Cruise":
                    return new Cruise();
                case "Mustang":
                    return new Mustang();
                default:
                    return null;
            }
        }
    }
}

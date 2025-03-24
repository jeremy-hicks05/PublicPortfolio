using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactory
{
    internal abstract class Car
    {
        public string Make { get; set; }

        public abstract void Drive();
    }
}

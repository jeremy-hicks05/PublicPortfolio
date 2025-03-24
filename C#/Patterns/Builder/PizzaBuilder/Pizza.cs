using MyPizzaBuilder.Ingredients;
using MyPizzaBuilder.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPizzaBuilder
{
    internal class Pizza
    {
        public Cheese Cheese { get; set; }
        public Pepperoni Pepperoni { get; set; }
        public Mushrooms Mushrooms { get; set; }
        public Sauce Sauce { get; set; }

        public Pizza(PizzaBuilder pizzaBuilder) 
        {
            Cheese = pizzaBuilder.Cheese;
            Pepperoni = pizzaBuilder.Pepperoni;
            Mushrooms = pizzaBuilder.Mushrooms;
            Sauce = pizzaBuilder.Sauce;
        }

    }
}

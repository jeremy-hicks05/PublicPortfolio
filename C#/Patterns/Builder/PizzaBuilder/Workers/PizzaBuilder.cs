using MyPizzaBuilder.Ingredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPizzaBuilder.Workers
{
    internal class PizzaBuilder
    {
        public Cheese Cheese { get; set; }
        public Pepperoni Pepperoni { get; set; }
        public Mushrooms Mushrooms { get; set; }
        public Sauce Sauce { get; set; }

        public PizzaBuilder WithCheese(Cheese cheese)
        {
            Cheese = cheese;
            return this;
        }

        public PizzaBuilder WithPepperoni(Pepperoni pepperoni)
        {
            Pepperoni = pepperoni;
            return this;
        }

        public PizzaBuilder WithMushrooms(Mushrooms mushrooms)
        {
            Mushrooms = mushrooms;
            return this;
        }

        public PizzaBuilder WithSauce(Sauce sauce) 
        {
            Sauce = sauce;
            return this;
        }

        public Pizza Build()
        {
            return new Pizza(this);
        }
    }
}

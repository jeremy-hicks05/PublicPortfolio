namespace MyPizzaBuilder
{
    using MyPizzaBuilder.Workers;
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Pizza Builder");
            Console.WriteLine("Please select ingredients");

            PizzaBuilder pizzaBuilder = new PizzaBuilder();
            pizzaBuilder.WithPepperoni(new Ingredients.Pepperoni())
                        .WithCheese(new Ingredients.Cheese())
                        .WithMushrooms(new Ingredients.Mushrooms());
            
            Pizza myPizza = pizzaBuilder.Build();
        }
    }
}
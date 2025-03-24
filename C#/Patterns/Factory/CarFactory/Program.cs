namespace CarFactory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Building two cars");

            CarFactory carFactory = new();

            Car firstCar = carFactory.GetCarType("Cruise");

            Car secondCar = carFactory.GetCarType("Mustang");

            List<Car> cars = new() {firstCar , secondCar};

            foreach(Car car in cars)
            {
                Console.WriteLine(car.Make);
                car.Drive();
            }

            foreach (Cruise cruise in cars.Where(c => c.GetType() == typeof(Cruise)))
            {
                Console.WriteLine(cruise.Make);
                cruise.Drive();
            }
        }
    }
}
namespace FlashlightAndBattery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Flashlight myFlashlight = new Flashlight(
                new Battery()
                {
                    Brand = "Duracell",
                    HoursToCharge = 4,
                });

            Flashlight yourFlashlight = new Flashlight(
                new Battery()
                {
                    Brand = "Energizer",
                    HoursToCharge = 6,
                });

            List<Flashlight> flashlights = new List<Flashlight>();
            flashlights.Add(myFlashlight);
            flashlights.Add( yourFlashlight);

            foreach(Flashlight flashlight in flashlights)
            {
                flashlight.CheckBattery();
                flashlight.Charge();
            }
        }
    }
}
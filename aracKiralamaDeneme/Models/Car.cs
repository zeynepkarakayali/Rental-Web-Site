namespace aracKiralamaDeneme.Models
{
    public class Car : Vehicle
    {
        public int DoorCount { get; set; } = 4;
        public string BodyType { get; set; } = "Sedan"; // Sedan, Hatchback, Coupe, Convertible, Wagon
        public bool HasSunroof { get; set; }
        public bool HasNavigationSystem { get; set; }
        public string SafetyRating { get; set; } = "5-Star"; // NCAP rating

        public override decimal CalculateRentalPrice(int rentalDays)
        {
            return rentalDays * DailyRentalPrice;
        }
    }
}

namespace aracKiralamaDeneme.Models
{
    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; set; }
        public override decimal CalculateRentalPrice(int rentalDays)
        {
            return (rentalDays * DailyRentalPrice) + (rentalDays * 200);
        }
    }
}

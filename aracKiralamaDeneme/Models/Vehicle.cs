namespace aracKiralamaDeneme.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
        public string FuelType { get; set; }
        public decimal DailyRentalPrice { get; set; }
        public string Status { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }


        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public bool IsPopular { get; set; }

        // Navigation property
        public ICollection<Rental> Rental { get; set; }

        public virtual decimal CalculateRentalPrice(int rentalDays)
        {
            return rentalDays * DailyRentalPrice;
        }
    }
}

namespace aracKiralamaDeneme.Models
{
    public class Motorcycle : Vehicle
    {
        public int EngineDisplacement {  get; set; }

        public override decimal CalculateRentalPrice(int rentalDays)
        {
            // Temel günlük fiyattan 50₺ indirimli ücret hesaplama
            decimal effectivePrice = DailyRentalPrice - 50;
            if (effectivePrice < 0) effectivePrice = 0; // Fiyatın negatif olmasını engelleme

            return rentalDays * effectivePrice;
        }
    }
}

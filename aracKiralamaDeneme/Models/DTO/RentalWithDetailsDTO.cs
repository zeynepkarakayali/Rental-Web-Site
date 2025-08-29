namespace aracKiralamaDeneme.Models.DTO
{
    public class RentalWithDetailsDto
    {
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }

        // Araç bilgileri
        public string Brand { get; set; }
        public string Model { get; set; }
    }

}

using System.ComponentModel.DataAnnotations;

namespace aracKiralamaDeneme.Models
{
    public class RentalDetails
    {
        [Key]
        public int RentalDetailId { get; set; }
        public int RentalId { get; set; }
        public Rental Rental {  get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }
        public string? Status { get; set; } // "Devam Ediyor", "Tamamlandı"

    }
}

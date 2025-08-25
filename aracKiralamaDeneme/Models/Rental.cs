using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace aracKiralamaDeneme.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Lütfen başlangıç tarihini seçiniz")]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Lütfen bitiş tarihini seçiniz")]
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Lütfen adresinizi giriniz")]
        [StringLength(100)]
        [Display(Name = "Adres Satırı 1")]
        public string AddressLine1 { get; set; } = string.Empty;

        [Display(Name = "Adres Satırı 2")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "Lütfen posta kodunu giriniz")]
        [Display(Name = "Posta Kodu")]
        [StringLength(10, MinimumLength = 4)]
        public string ZipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen şehrinizi giriniz")]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [StringLength(10)]
        public string? State { get; set; }

        [Required(ErrorMessage = "Lütfen ülkenizi giriniz")]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;


        // Navigation Properties
        public Customer Customer { get; set; }
        public List<RentalDetails> RentalDetails { get; set; } = new List<RentalDetails>();
        public Vehicle Vehicle { get; set; }  // <- Vehicle navigation property
    }
}

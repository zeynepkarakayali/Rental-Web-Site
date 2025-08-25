using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace aracKiralamaDeneme.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // Araç bilgileri
        public int VehicleId { get; set; }
        public string? LongDescription { get; set; }
        public string? ShortDescription { get; set; }
        public decimal DailyRentalPrice { get; set; }
        public string Brand { get; internal set; }
        public string Model { get; internal set; }
        public string? ImageUrl { get; internal set; }
        public string PlateNumber { get; internal set; }


        // Tarihler
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }
        public decimal TotalPrice { get; set; }  // <-- Toplam fiyat

        // Müşteri bilgileri
        public int CustomerId { get; set; }
        [Required] public string FirstName { get; set; }  // <-- Yeni eklendi
        [Required] public string LastName { get; set; }   // <-- Yeni eklendi
        [Required, EmailAddress] public string Email { get; set; }  // <-- Yeni eklendi
        [Required] public string PhoneNumber { get; set; }  // <-- Yeni eklendi

        // Adres bilgileri
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        // Rental bilgileri
        public int RentalId { get; set; }

        //public RentalDetails RentalDetails { get; set; }

    }
}

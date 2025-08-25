namespace aracKiralamaDeneme.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseType { get; set; } // "B", "A2", "CE" vb.
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Foreign Key
        public int? AddressId { get; set; }

        // Navigation property
        public ICollection<RentalDetails> RentalDetails { get; set; } = new List<RentalDetails>();
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        // Identity ile ilişki
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}

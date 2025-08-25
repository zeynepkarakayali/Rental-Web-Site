namespace aracKiralamaDeneme.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        // İlişki
        public int CustomerId { get; set; }
        // public List<Customer> Customers { get; set; } = new List<Customer>(); many-to-many ilişki için bu kullanılabilir ama profilde çok sıkıntı çıkarıyor
        public Customer Customer { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aracKiralamaDeneme.Models
{
    public class CarRentalContext : IdentityDbContext<ApplicationUser>
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetails> RentalDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasBaseType<Vehicle>();
            modelBuilder.Entity<Motorcycle>().HasBaseType<Vehicle>();
            modelBuilder.Entity<Truck>().HasBaseType<Vehicle>();

            modelBuilder.Entity<Vehicle>()
                .HasDiscriminator<string>("VehicleType")
                .HasValue<Car>("Otomobil")
                .HasValue<Motorcycle>("Motosiklet")
                .HasValue<Truck>("Kamyonet");

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.ApplicationUser)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.ApplicationUserId);

            modelBuilder.Entity<Address>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine restrict
        }
    }

    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        // Eğer ekstra alan eklemek istersen (FirstName, LastName vb.)
        public Customer? Customer { get; set; }

    }
}

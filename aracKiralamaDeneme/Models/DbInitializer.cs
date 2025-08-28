//using aracKiralamaDeneme.Models;
//using aracKiralamaDeneme.Repositories;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Linq;

//namespace aracKiralamaDeneme.Models
//{
//    public static class DbInitializer
//    {
//        public static async Task InitializeAsync(IServiceProvider serviceProvider)
//        {
//            using var scope = serviceProvider.CreateScope();

//            var vehicleRepo = scope.ServiceProvider.GetRequiredService<VehicleRepository>();
//            var customerRepo = scope.ServiceProvider.GetRequiredService<CustomerRepository>();
//            var rentalRepo = scope.ServiceProvider.GetRequiredService<RentalRepository>();

//            // Araç ekle
//            if ((await vehicleRepo.GetAllAsync()).Any() == false)
//            {
//                await vehicleRepo.AddAsync(new Car
//                {
//                    Brand = "Fiat",
//                    Model = "Aegea",
//                    PlateNumber = "34ABC123",
//                    FuelType = "Benzin",
//                    DailyRentalPrice = 500,
//                    Status = "Müsait",
//                    ShortDescription = "Konforlu ve şık",
//                    LongDescription = "Uzun yol ve şehir içi için ideal bir otomobil.",
//                    ImageUrl = "/images/car1.png",
//                    ImageThumbnailUrl = "/images/car1.png",
//                    IsPopular = true
//                });

//                await vehicleRepo.AddAsync(new Motorcycle
//                {
//                    Brand = "Yamaha",
//                    Model = "R25",
//                    PlateNumber = "34MOTO25",
//                    FuelType = "Benzin",
//                    DailyRentalPrice = 300,
//                    Status = "Müsait",
//                    EngineDisplacement = 250,
//                    ShortDescription = "Hızlı ve hafif motosiklet",
//                    LongDescription = "Şehir trafiğinde pratik, uzun yolda keyifli.",
//                    ImageUrl = "/images/motorsiklet.jpg",
//                    ImageThumbnailUrl = "/images/motorsiklet.jpg",
//                    IsPopular = true
//                });

//                await vehicleRepo.AddAsync(new Truck
//                {
//                    Brand = "Ford",
//                    Model = "F-Max",
//                    PlateNumber = "34TRK456",
//                    FuelType = "Dizel",
//                    DailyRentalPrice = 800,
//                    Status = "Müsait",
//                    LoadCapacity = 10000,
//                    ShortDescription = "Konforlu ve hızlı bir kamyon",
//                    LongDescription = "Yüksek ağırlıklara dayanıklı ve sürüş keyfi yüksek",
//                    ImageUrl = "/images/truck.jpg",
//                    ImageThumbnailUrl = "/images/truck.jpg",
//                    IsPopular = true
//                });

//                await vehicleRepo.SaveAsync();
//            }
//            else
//            {
//                // Mevcut kayıtların NULL alanlarını doldur
//                var allVehicles = await vehicleRepo.GetAllAsync();
//                foreach (var vehicle in allVehicles)
//                {
//                    vehicle.ShortDescription ??= "Açıklama yok";
//                    vehicle.LongDescription ??= "Detaylı açıklama yok.";
//                    vehicle.ImageUrl ??= "/images/default.jpg";
//                    vehicle.ImageThumbnailUrl ??= "/images/default.jpg";

//                    // IsPopular varsayılan false olabilir
//                    // Burada set etmezsen olduğu gibi kalır
//                }

//                await vehicleRepo.SaveAsync();
//            }

//            // Müşteri ekle
//            if ((await customerRepo.GetAllAsync()).Any() == false)
//            {
//                await customerRepo.AddAsync(new Customer
//                {
//                    FirstName = "Ahmet",
//                    LastName = "Yılmaz",
//                    LicenseNumber = "ABC12345",
//                    LicenseType = "A2",
//                    PhoneNumber = "05551112233",
//                    Email = "ayilmaz@gmail.com"
//                });

//                await customerRepo.AddAsync(new Customer
//                {
//                    FirstName = "Ayşe",
//                    LastName = "Demir",
//                    LicenseNumber = "XYZ67890",
//                    LicenseType = "B",
//                    PhoneNumber = "05003334455",
//                    Email = "aysedemir@gmail.com"
//                });

//                await customerRepo.SaveAsync();
//            }
//        }
//    }
//}

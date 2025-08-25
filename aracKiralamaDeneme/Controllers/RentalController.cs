using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aracKiralamaDeneme.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly CarRentalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalController(CarRentalContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }


        // GET: Rental/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout(int vehicleId, DateTime? startDate, DateTime? endDate)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
                return NotFound();

            // Giriş yapmış kullanıcı
            var userId = _userManager.GetUserId(User);
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Eğer bir adres varsa ilkini kullanabiliriz
            var address = customer.Addresses.FirstOrDefault();

            var totalDays = ((endDate ?? DateTime.Today) - (startDate ?? DateTime.Today)).Days;
            decimal totalPrice = vehicle.CalculateRentalPrice(totalDays);

            var model = new CheckoutViewModel
            {
                VehicleId = vehicle.VehicleId,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ImageUrl = vehicle.ImageUrl,
                PlateNumber = vehicle.PlateNumber,
                LongDescription = vehicle.LongDescription,
                ShortDescription = vehicle.ShortDescription,
                DailyRentalPrice = vehicle.DailyRentalPrice,

                StartDate = startDate ?? DateTime.Today,
                EndDate = endDate ?? DateTime.Today,
                TotalDays = totalDays,
                TotalPrice = totalPrice,

                // Customer bilgileri
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,

                // Adres bilgileri (varsa)
                AddressLine1 = address?.AddressLine1,
                AddressLine2 = address?.AddressLine2,
                City = address?.City,
                ZipCode = address?.ZipCode,
                Country = address?.Country
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage("/Account/Login");

            // Customer'ı getir
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

            if (customer == null)
                return NotFound("Customer not found");

            // Araç bilgilerini getir
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == model.VehicleId);

            if (vehicle == null || vehicle.Status != "Available")
            {
                ModelState.AddModelError("", "Araç kiralamaya uygun değil");
                return View(model);
            }

            var totalDays = (model.EndDate - model.StartDate).Days;
            decimal calculatedPrice = vehicle.CalculateRentalPrice(totalDays);

            // Rental oluştur
            var rental = new Rental
            {
                CustomerId = customer.CustomerId,
                VehicleId = vehicle.VehicleId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                ZipCode = model.ZipCode,
                Country = model.Country
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            // RentalDetails oluştur, fiyatı Rental ve Vehicle üzerinden hesapla
            var rentalDetail = new RentalDetails
            {
                RentalId = rental.RentalId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                TotalPrice = calculatedPrice,
                Status = "Devam Ediyor"
            };

            _context.RentalDetails.Add(rentalDetail);
            await _context.SaveChangesAsync();

            // Araç durumunu güncelle
            vehicle.Status = "Rented";
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { rentalId = rental.RentalId });
        }


        // --------------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCheckout(CheckoutViewModel model)
        {
            ModelState.Remove("Brand");
            ModelState.Remove("Model");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("PlateNumber");
            ModelState.Remove("LongDescription");
            ModelState.Remove("ShortDescription");
            ModelState.Remove("DailyRentalPrice");

            var userId = _userManager.GetUserId(User);

            // Müşteriyi doğrudan ApplicationUserId üzerinden bul
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (customer == null)
            {
                // Hata durumunda, uygun bir işlem yap
                return NotFound("Müşteri bulunamadı. Lütfen giriş yaptığınızdan emin olun.");
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == model.VehicleId);

            // ModelState geçersizse araç bilgilerini doldur ve Checkout view'a geri dön
            if (!ModelState.IsValid)
            {
                model.Brand = vehicle.Brand;
                model.Model = vehicle.Model;
                model.ImageUrl = vehicle.ImageUrl;
                model.PlateNumber = vehicle.PlateNumber;
                model.LongDescription = vehicle.LongDescription;
                model.ShortDescription = vehicle.ShortDescription;
                model.DailyRentalPrice = vehicle.DailyRentalPrice;

                return View("Checkout", model);
            }

            // Araç durumunu kontrol et
            //if (vehicle == null || vehicle.Status != "Available")
            //{
            //    ModelState.AddModelError("", "Araç kiralamaya uygun değil.");
            //    return View("Checkout", model);
            //}

            // Yeni Rental oluştur
            var rental = new Rental
            {
                CustomerId = customer.CustomerId,
                VehicleId = vehicle.VehicleId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                ZipCode = model.ZipCode,
                Country = model.Country
            };


            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            // RentalDetails oluştur
            var rentalDetails = new RentalDetails
            {
                RentalId = rental.RentalId,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                TotalPrice = model.TotalPrice,
                Status = "Devam Ediyor"
            };

            _context.RentalDetails.Add(rentalDetails);
            await _context.SaveChangesAsync();

            var existingAddress = customer.Addresses.FirstOrDefault();
            if (existingAddress != null)
            {
                existingAddress.AddressLine1 = model.AddressLine1;
                existingAddress.AddressLine2 = model.AddressLine2;
                existingAddress.City = model.City;
                existingAddress.ZipCode = model.ZipCode;
                existingAddress.Country = model.Country;
            }
            else
            {
                var newAddress = new Address
                {
                    CustomerId = customer.CustomerId,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    ZipCode = model.ZipCode,
                    Country = model.Country
                };
                _context.Addresses.Add(newAddress);
            }

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Araç durumunu güncelle
            vehicle.Status = "Kirada";
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success", new { rentalId = rental.RentalId });
        }

        [HttpGet]
        public async Task<IActionResult> Success(int rentalId)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.RentalId == rentalId);

            if (rental == null) return NotFound();

            return View(rental);
        }


        // --------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CancelRental([FromBody] int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.RentalDetails)
                .FirstOrDefaultAsync(r => r.RentalId == id);

            if (rental == null)
                return NotFound();

            // RentalDetails'leri iptal et
            foreach (var detail in rental.RentalDetails)
                detail.Status = "İptal";

            // Aracın durumu tekrar "Müsait" yapılıyor
            rental.Vehicle.Status = "Müsait";

            _context.Update(rental);
            await _context.SaveChangesAsync();

            // JSON dön, JS yönlendirmeyi yapacak
            return Ok(new { success = true });
        }
    }
}

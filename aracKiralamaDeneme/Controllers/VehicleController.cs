using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace aracKiralamaDeneme.Controllers
{
    public class VehicleController : Controller
    {
        private readonly CarRentalContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehicleController(CarRentalContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // /Vehicle
        public IActionResult Index()
        {
            var vehicles = _context.Vehicles.ToList();

            return View(vehicles);
        }

        //public IActionResult Details(int id)
        //{
        //    var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehicleId == id);

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vehicle);
        //}

        public async Task<IActionResult> Details(int id)
        {
            //Vehicle vehicle = null;

            //// Önce Car mı diye kontrol edelim
            //vehicle = await _context.Vehicles
            //    .OfType<Car>()
            //    .FirstOrDefaultAsync(v => v.VehicleId == id);

            //if (vehicle == null)
            //{
            //    // Car değilse Motorcycle mı diye bak
            //    vehicle = await _context.Vehicles
            //        .OfType<Motorcycle>()
            //        .FirstOrDefaultAsync(v => v.VehicleId == id);
            //}

            //if (vehicle == null)
            //{
            //    // Truck mı diye bak
            //    vehicle = await _context.Vehicles
            //        .OfType<Truck>()
            //        .FirstOrDefaultAsync(v => v.VehicleId == id);
            //}

            //if (vehicle == null)
            //    return NotFound();

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            // Login olmuş user'ı al
            var user = await _userManager.GetUserAsync(User);
            Customer customer = null;
            if (user != null)
            {
                customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            }

            // Ehliyet uyum kontrolü
            bool canRent = false;
            if (customer != null)
            {
                canRent = IsLicenseValidForVehicle(customer.LicenseType, vehicle);
            }

            // ViewModel oluştur
            var viewModel = new VehicleDetailsViewModel
            {
                Vehicle = vehicle,
                CanRent = canRent
            };

            return View(viewModel);
        }

        //public async Task<IActionResult> Details(int id)
        //{
        //    Vehicle vehicle = null;

        //    // Önce Car mı kontrol et
        //    var car = await _context.Vehicles.OfType<Car>()
        //        .FirstOrDefaultAsync(v => v.VehicleId == id);
        //    if (car != null)
        //    {
        //        vehicle = car;
        //    }

        //    // Motorcycle mı kontrol et
        //    var moto = await _context.Vehicles.OfType<Motorcycle>()
        //        .FirstOrDefaultAsync(v => v.VehicleId == id);
        //    if (moto != null)
        //    {
        //        vehicle = moto;
        //    }

        //    // Truck mı kontrol et
        //    var truck = await _context.Vehicles.OfType<Truck>()
        //        .FirstOrDefaultAsync(v => v.VehicleId == id);
        //    if (truck != null)
        //    {
        //        vehicle = truck;
        //    }

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    // Login olmuş user'ı al
        //    var user = await _userManager.GetUserAsync(User);
        //    Customer customer = null;
        //    if (user != null)
        //    {
        //        customer = await _context.Customers
        //            .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
        //    }

        //    // Ehliyet uyum kontrolü
        //    bool canRent = false;
        //    if (customer != null)
        //    {
        //        canRent = IsLicenseValidForVehicle(customer.LicenseType, vehicle);
        //    }

        //    // ViewModel oluştur
        //    var viewModel = new VehicleDetailsViewModel
        //    {
        //        Vehicle = vehicle,
        //        CanRent = canRent
        //    };

        //    return View(viewModel);
        //}


        // Ehliyet uygunluk kontrolü ayrı metot
        private bool IsLicenseValidForVehicle(string licenseType, Vehicle vehicle)
        {
            if (vehicle is Car && (licenseType == "B" || licenseType == "B2"))
                return true;

            if (vehicle is Motorcycle && (licenseType == "A2" || licenseType == "A"))
                return true;

            if (vehicle is Truck && licenseType == "C")
                return true;

            return false;
        }



        //[HttpGet]
        //public IActionResult CheckAvailability(int vehicleId, DateTime startDate, DateTime endDate)
        //{
        //    // Burada araç kiralanmış mı kontrol edelim
        //    bool isAvailable = !_context.Rentals
        //        .Any(r =>
        //            r.VehicleId == vehicleId &&
        //            r.StartDate < endDate &&
        //            r.EndDate > startDate);

        //    var statusMessage = isAvailable ? "Müsait" : "Kirada";

        //    return Json(new { available = isAvailable, message = statusMessage });
        //}

        [HttpGet]
        public async Task<IActionResult> CheckAvailability(int vehicleId, DateTime startDate, DateTime endDate)
        {
            var hasActiveRental = await _context.RentalDetails
                .Include(rd => rd.Rental)
                .AnyAsync(rd => rd.Rental.VehicleId == vehicleId
                             && rd.Status != "İptal"   // 🔹 iptal edilenler hesaba katılmıyor
                             && (
                                    (startDate >= rd.StartDate && startDate <= rd.EndDate) ||
                                    (endDate >= rd.StartDate && endDate <= rd.EndDate) ||
                                    (startDate <= rd.StartDate && endDate >= rd.EndDate)
                                )
                );

            if (hasActiveRental)
            {
                return Json(new { available = false, message = "Dolu" });
            }
            else
            {
                return Json(new { available = true, message = "Boşta" });
            }
        }

        [HttpGet]
        public IActionResult GetUnavailableDates(int vehicleId)
        {
            // Araç kiralamalarını al
            var rentals = _context.Rentals
                .Where(r => r.VehicleId == vehicleId)
                .Select(r => new { r.StartDate, r.EndDate })
                .ToList();

            // Tarihleri diziye çevir
            var dates = new List<string>();
            foreach (var r in rentals)
            {
                for (var d = r.StartDate; d <= r.EndDate; d = d.AddDays(1))
                {
                    dates.Add(d.ToString("yyyy-MM-dd"));
                }
            }

            return Json(dates);
        }


    }
}

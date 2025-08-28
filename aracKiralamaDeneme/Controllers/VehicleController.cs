using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace aracKiralamaDeneme.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IDbConnection _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehicleController(IDbConnection db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Vehicle
        public async Task<IActionResult> Index()
        {
            // SP: GetAllVehicles
            var vehicles = await _db.QueryAsync<Vehicle>("EXEC GetAllVehicles");
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
            // SP: GetVehicleById
            var vehicle = await _db.QuerySingleOrDefaultAsync<Vehicle>(
                "EXEC GetVehicleById @Id",
                new { Id = id }
            );

            if (vehicle == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            Customer customer = null;
            if (user != null)
            {
                // SP: GetCustomerByUserId
                customer = await _db.QuerySingleOrDefaultAsync<Customer>(
                    "EXEC GetCustomerByUserId @UserId",
                    new { UserId = user.Id }
                );
            }

            bool canRent = customer != null && IsLicenseValidForVehicle(customer.LicenseType, vehicle);

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
            // SP: CheckVehicleAvailability
            var hasActiveRental = await _db.QueryFirstOrDefaultAsync<int>(
                "EXEC CheckVehicleAvailability @VehicleId, @StartDate, @EndDate",
                new { VehicleId = vehicleId, StartDate = startDate, EndDate = endDate }
            );

            if (hasActiveRental > 0)
                return Json(new { available = false, message = "Dolu" });
            else
                return Json(new { available = true, message = "Boşta" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUnavailableDates(int vehicleId)
        {
            // SP: GetRentalDatesByVehicle
            var rentals = await _db.QueryAsync<(DateTime StartDate, DateTime EndDate)>(
                "EXEC GetRentalDatesByVehicle @VehicleId",
                new { VehicleId = vehicleId }
            );

            var dates = new List<string>();
            foreach (var r in rentals)
            {
                for (var d = r.StartDate; d <= r.EndDate; d = d.AddDays(1))
                    dates.Add(d.ToString("yyyy-MM-dd"));
            }

            return Json(dates);
        }
    }
}
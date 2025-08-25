using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.ViewModels;
using aracKiralamaDeneme.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace aracKiralamaDeneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly CarRentalContext _context;

        public HomeController(CarRentalContext context)
        {
            _context = context;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            // Tüm araçları çek
            var vehicles = await _context.Vehicles
                .AsNoTracking()
                .ToListAsync();

            return View(vehicles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}

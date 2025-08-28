using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Models.ViewModels;
using aracKiralamaDeneme.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace aracKiralamaDeneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly VehicleRepository _vehicleRepository;

        public HomeController(VehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            // Tüm araçları çek
            var vehicles = await _vehicleRepository.GetAllAsync();
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

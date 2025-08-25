using aracKiralamaDeneme.Models;
using System.Collections.Generic;

namespace aracKiralamaDeneme.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Vehicle> PopularVehicles { get; set; }
        public IEnumerable<Vehicle> AvailableVehicles { get; set; }
    }
}

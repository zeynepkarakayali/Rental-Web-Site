using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Implementations
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(CarRentalContext context) : base(context) { }

        // Popüler araçlar
        public async Task<IEnumerable<Vehicle>> GetPopularVehiclesAsync()
        {
            return await _dbSet.Where(v => v.IsPopular).ToListAsync();
        }

        // Müsait araçlar
        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _dbSet.Where(v => v.Status == "Müsait").ToListAsync();
        }

        // Yakıt tipine göre araçlar
        public async Task<IEnumerable<Vehicle>> GetByFuelTypeAsync(string fuelType)
        {
            return await _dbSet.Where(v => v.FuelType == fuelType).ToListAsync();
        }
    }
}

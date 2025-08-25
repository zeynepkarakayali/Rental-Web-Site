using aracKiralamaDeneme.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetPopularVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetByFuelTypeAsync(string fuelType);
    }
}

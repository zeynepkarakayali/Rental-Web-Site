using aracKiralamaDeneme.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories
{
    public class VehicleRepository
    {
        private readonly IDbConnection _connection;
        public VehicleRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // Bütün araçları getir
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _connection.QueryAsync<Vehicle>(
                "GetAllVehicles",
                commandType: CommandType.StoredProcedure
            );
        }
        // Popüler araçlar
        public async Task<IEnumerable<Vehicle>> GetPopularVehiclesAsync()
        {
            return await _connection.QueryAsync<Vehicle>(
                "GetPopularVehicles",
                commandType: CommandType.StoredProcedure
            );
        }

        // Müsait araçlar
        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await _connection.QueryAsync<Vehicle>(
                "GetAvailableVehicles",
                commandType: CommandType.StoredProcedure
            );
        }

        // Yakıt tipine göre araçlar
        public async Task<IEnumerable<Vehicle>> GetByFuelTypeAsync(string fuelType)
        {
            return await _connection.QueryAsync<Vehicle>(
                "GetVehiclesByFuelType",
                new { FuelType = fuelType },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

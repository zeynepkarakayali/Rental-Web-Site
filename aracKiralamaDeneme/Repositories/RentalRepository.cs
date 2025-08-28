using aracKiralamaDeneme.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories
{
    public class RentalRepository
    {
        private readonly IDbConnection _connection;
        public RentalRepository(IDbConnection connection) {
            _connection = connection;
        }

        // Aktif kiralamalar
        public async Task<IEnumerable<RentalDetails>> GetActiveRentalsAsync()
        {
            var rentals = await _connection.QueryAsync<RentalDetails, Rental, RentalDetails>(
                "GetActiveRentals",
                (rd, r) => { rd.Rental = r; return rd; },
                commandType: CommandType.StoredProcedure,
                splitOn: "Id"
            );
            return rentals;
        }

        public async Task<IEnumerable<RentalDetails>> GetRentalsByCustomerIdAsync(int customerId)
        {
            var rentals = await _connection.QueryAsync<RentalDetails, Rental, RentalDetails>(
                "GetRentalsByCustomerId",
                (rd, r) => { rd.Rental = r; return rd; },
                param: new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure,
                splitOn: "Id"
            );
            return rentals;
        }
    }
}

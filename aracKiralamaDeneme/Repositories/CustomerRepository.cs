using aracKiralamaDeneme.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories
{
    public class CustomerRepository
    {
        private readonly IDbConnection _connection;

        public CustomerRepository(IDbConnection connection)
        {
            _connection = connection;
        }


        public async Task<IEnumerable<Customer>> GetByLicenseTypeAsync(string licenseType)
        {
            return await _connection.QueryAsync<Customer>(
                "GetCustomersByLicenseType",
                new { LicenseType = licenseType },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CarRentalContext context) : base(context) { }

        // Ehliyet tipine göre müşteri listesi
        public async Task<IEnumerable<Customer>> GetByLicenseTypeAsync(string licenseType)
        {
            return await _dbSet.Where(c => c.LicenseType == licenseType).ToListAsync();
        }
    }
}

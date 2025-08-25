using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Repositories.Implementations;
using aracKiralamaDeneme.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Implementations
{
    public class RentalRepository : Repository<RentalDetails>, IRentalRepository
    {
        public RentalRepository(CarRentalContext context) : base(context) { }

        // Aktif kiralamalar
        public async Task<IEnumerable<RentalDetails>> GetActiveRentalsAsync()
        {
            return await _dbSet
                .Include(rd => rd.Rental)
                .Where(rd => rd.Rental.EndDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<RentalDetails>> GetRentalsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Include(rd => rd.Rental)
                .Where(rd => rd.Rental.CustomerId == customerId)
                .ToListAsync();
        }
    }
}

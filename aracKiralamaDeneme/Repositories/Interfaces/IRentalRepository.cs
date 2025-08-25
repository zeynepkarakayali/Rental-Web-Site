using aracKiralamaDeneme.Models;
using aracKiralamaDeneme.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Interfaces
{
    public interface IRentalRepository : IRepository<RentalDetails>
    {
        Task<IEnumerable<RentalDetails>> GetActiveRentalsAsync();
        Task<IEnumerable<RentalDetails>> GetRentalsByCustomerIdAsync(int customerId);
    }
}
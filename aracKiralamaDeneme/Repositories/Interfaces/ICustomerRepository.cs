using aracKiralamaDeneme.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aracKiralamaDeneme.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetByLicenseTypeAsync(string licenseType);
    }
}

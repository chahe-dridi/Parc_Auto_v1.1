using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IAssuranceService
    {
        Task<List<Assurance>> GetAllAssurancesAsync();
        Task<Assurance> GetAssuranceByIdAsync(int id);
        Task AddAssuranceAsync(Assurance assurance);
        Task UpdateAssuranceAsync(Assurance assurance);
        Task DeleteAssuranceAsync(int id);
        Task<bool> AssuranceExistsAsync(int id); // Add this method
 
    }
}

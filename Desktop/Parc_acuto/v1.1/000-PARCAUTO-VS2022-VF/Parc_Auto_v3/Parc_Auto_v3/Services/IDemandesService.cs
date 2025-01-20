using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IDemandesService
    {
        Task<List<Demandes>> GetAllDemandesAsync(); // Change return type to List
        Task<Demandes> GetDemandeByIdAsync(int id);
        Task AddDemandeAsync(Demandes demande);
        Task UpdateDemandeAsync(Demandes demande);
        Task DeleteDemandeAsync(int id);

        Task<IEnumerable<Demandes>> GetDemandesByUserIdAsync(string userId);




      
    }
}

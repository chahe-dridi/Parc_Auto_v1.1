using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IModeleService
    {
        Task<IEnumerable<Modele>> GetAllModelesAsync();
        Task<Modele> GetModeleByIdAsync(int id);


        Task<List<Modele>> GetModelesByMarqueIdAsync(int marqueId);

        Task AddModeleAsync(Modele modele);
        Task UpdateModeleAsync(Modele modele);
       Task DeleteModeleAsync(int modeleId);

	}
}

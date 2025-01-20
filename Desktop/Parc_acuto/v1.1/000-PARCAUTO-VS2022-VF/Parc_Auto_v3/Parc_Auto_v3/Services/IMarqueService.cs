using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IMarqueService
    {
        Task<IEnumerable<Marque>> GetAllMarquesAsync();
        Task<Marque> GetMarqueByIdAsync(int id);
        Task AddMarqueAsync(Marque marque);
        Task UpdateMarqueAsync(Marque marque);
        Task DeleteMarqueAsync(int marqueId);

	}
}

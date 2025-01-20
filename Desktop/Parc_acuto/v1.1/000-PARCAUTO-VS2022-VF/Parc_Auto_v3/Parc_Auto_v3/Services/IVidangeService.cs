using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IVidangeService
    {
        Task<IEnumerable<Vidange>> GetAllVidangesAsync();
        Task<Vidange> GetVidangeByIdAsync(int id);
        Task AddVidangeAsync(Vidange vidange);
        Task UpdateVidangeAsync(Vidange vidange);
        Task DeleteVidangeAsync(int id);
    }
}

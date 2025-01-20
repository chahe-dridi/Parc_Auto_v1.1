using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IVisiteTechniqueService
    {
        Task<List<VisiteTechnique>> GetAllVisiteTechniquesAsync();
        Task<VisiteTechnique> GetVisiteTechniqueByIdAsync(int id);
        Task AddVisiteTechniqueAsync(VisiteTechnique visiteTechnique);
        Task UpdateVisiteTechniqueAsync(VisiteTechnique visiteTechnique);
        Task DeleteVisiteTechniqueAsync(int id);
        Task<bool> VisiteTechniqueExistsAsync(int id);
    }
}

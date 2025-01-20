using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public interface IVoitureService
    {
        Task<List<Voiture>> GetAllVoituresAsync();
        Task<Voiture> GetVoitureByIdAsync(int id);
        Task AddVoitureAsync(Voiture voiture);
        Task UpdateVoitureAsync(Voiture voiture);
        Task DeleteVoitureAsync(int id);

        Task<List<Voiture>> GetAvailableVoituresAsync();


        //  Task<decimal> GetTotalMoneySpentOnVoitureAsync(int id);
        Task<decimal> GetTotalAssuranceCostAsync(int voitureId);
        Task<decimal> GetTotalVidangeCostAsync(int voitureId);
        Task<decimal> GetTotalVignetteCostAsync(int voitureId);

        Task<decimal> GetTotalVisiteTechniqueCostAsync(int voitureId);




       
        Task<List<Voiture>> GetAvailableVoituresByTypeAsync(string typeVoiture);


        Task<List<Demandes>> GetDemandesByVoitureIdAsync(int voitureId);

    }
}

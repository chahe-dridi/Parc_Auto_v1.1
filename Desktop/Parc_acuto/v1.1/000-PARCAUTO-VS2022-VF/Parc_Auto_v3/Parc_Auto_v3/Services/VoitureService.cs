using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class VoitureService : IVoitureService
    {
        private readonly ApplicationDbContext _context;

        public VoitureService(ApplicationDbContext context)
        {
            _context = context;
        }
         

        public async Task<List<Voiture>> GetAllVoituresAsync()
        {
            return await _context.Voitures
                                 .Include(v => v.Marque)
                                 .Include(v => v.Modele)
                                 .ToListAsync();
        }

        public async Task<Voiture> GetVoitureByIdAsync(int id)
        {
            return await _context.Voitures
                                 .Include(v => v.Marque)
                                 .Include(v => v.Modele)
                                 .FirstOrDefaultAsync(v => v.Id == id);
        }

















       
 
        public async Task AddVoitureAsync(Voiture voiture)
        {
            _context.Add(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVoitureAsync(Voiture voiture)
        {
            _context.Update(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVoitureAsync(int id)
        {
            var voiture = await _context.Voitures.FindAsync(id);
            _context.Voitures.Remove(voiture);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VoitureExistsAsync(int id)
        {
            return await _context.Voitures.AnyAsync(e => e.Id == id);
        }

        // New method to get available cars
        public async Task<List<Voiture>> GetAvailableVoituresAsync()
        {
            return await _context.Voitures
                                 .Where(v => v.Disponibilite == "Disponible")
                                 .ToListAsync();
        }











         

        public async Task<decimal> GetTotalVisiteTechniqueCostAsync(int voitureId)
        {
            return await _context.VisiteTechniques
                .Where(vt => vt.VoitureId == voitureId)
                .SumAsync(vt => vt.PrixUnitaire);
        }

        public async Task<decimal> GetTotalVignetteCostAsync(int voitureId)
        {
            return await _context.Vignettes
                .Where(vg => vg.VoitureId == voitureId)
                .SumAsync(vg => vg.PrixUnitaire);
        }

        public async Task<decimal> GetTotalVidangeCostAsync(int voitureId)
        {
            return await _context.Vidanges
                .Where(vd => vd.VoitureId == voitureId)
                .SumAsync(vd => vd.MontantHt);
        }

        public async Task<decimal> GetTotalAssuranceCostAsync(int voitureId)
        {
            return await _context.Assurances
                .Where(asr => asr.VoitureId == voitureId)
                .SumAsync(asr => asr.PrixUnitaire);
        }







        public async Task<List<Voiture>> GetAvailableVoituresByTypeAsync(string typeVoiture)
        {
            return await _context.Voitures
                                 .Where(v => v.Disponibilite == "Disponible" && v.TypeVoiture == typeVoiture)
                                 .ToListAsync();
        }










        public async Task<List<Demandes>> GetDemandesByVoitureIdAsync(int voitureId)
        {
            return await _context.Demandes
                                 .Where(d => d.VoitureId == voitureId)
                                 .OrderBy(d => d.DateDepart)
                                 .ToListAsync();
        }



    }
}

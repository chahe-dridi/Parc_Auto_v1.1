using Parc_Auto_v3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class DemandesService : IDemandesService
    {
        private readonly ApplicationDbContext _context;

        public DemandesService(ApplicationDbContext context)
        {
            _context = context;
        }
 

        public async Task<List<Demandes>> GetAllDemandesAsync()
        {
            return await _context.Demandes
                                 .Include(d => d.Voiture) // Include the Voiture navigation property
                                 .ToListAsync();
        }

     


        public async Task<Demandes> GetDemandeByIdAsync(int id)
        {
            return await _context.Demandes
                .Include(d => d.Voiture) // Ensure the related Voiture is included
                .FirstOrDefaultAsync(d => d.Id == id);
        }

 
        public async Task<IEnumerable<Demandes>> GetDemandesByUserIdAsync(string userId)
        {
            return await _context.Demandes
                                 .Include(d => d.Voiture) // Include the Voiture navigation property
                                 .Where(d => d.UserId == userId) // Ensure the filtering by UserId
                                 .ToListAsync();
        }



        public async Task AddDemandeAsync(Demandes demande)
        {
            await _context.Demandes.AddAsync(demande); // Use AddAsync for consistency
            await _context.SaveChangesAsync();
        }

     


        public async Task UpdateDemandeAsync(Demandes updatedDemande)
        {
            // Retrieve the original Demande from the database
            var originalDemande = await _context.Demandes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == updatedDemande.Id);

            if (originalDemande != null)
            {
                // Preserve the original UserId
                updatedDemande.UserId = originalDemande.UserId;

                // Update the Demande
                _context.Entry(updatedDemande).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

 



        public async Task DeleteDemandeAsync(int id)
        {
            var demande = await _context.Demandes.FindAsync(id);
            if (demande != null)
            {
                _context.Demandes.Remove(demande);
                await _context.SaveChangesAsync();
            }
        }






















    }
}

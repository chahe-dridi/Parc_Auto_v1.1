using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class AssuranceService : IAssuranceService
    {
        private readonly ApplicationDbContext _context;

        public AssuranceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assurance>> GetAllAssurancesAsync()
        {
            return await _context.Assurances
                                 .Include(a => a.Voiture) // Eager load the Voiture entity
                                 .ToListAsync();
 
        }

        public async Task<Assurance> GetAssuranceByIdAsync(int id)
        {
            return await _context.Assurances
                                 .Include(a => a.Voiture) // Eager load the Voiture entity
                                 .FirstOrDefaultAsync(a => a.Id == id);
 
            return await _context.Assurances.FindAsync(id);
 
        }

        public async Task AddAssuranceAsync(Assurance assurance)
        {
            _context.Add(assurance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssuranceAsync(Assurance assurance)
        {
            _context.Update(assurance);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAssuranceAsync(int id)
        {
            var assurance = await _context.Assurances.FindAsync(id);
            if (assurance != null)
            {
                _context.Assurances.Remove(assurance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AssuranceExistsAsync(int id) // Implement this method
        {
            return await _context.Assurances.AnyAsync(a => a.Id == id);
        }
 
    }
}

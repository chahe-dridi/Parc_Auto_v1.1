using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class SinistreService : ISinistreService
    {
        private readonly ApplicationDbContext _context;

        public SinistreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sinistre>> GetAllSinistresAsync()
        {
            return await _context.Sinistres.Include(s => s.Voiture).ToListAsync();
        }

        public async Task<Sinistre> GetSinistreByIdAsync(int id)
        {
            return await _context.Sinistres.Include(s => s.Voiture).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSinistreAsync(Sinistre sinistre)
        {
            _context.Sinistres.Add(sinistre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSinistreAsync(Sinistre sinistre)
        {
            _context.Entry(sinistre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSinistreAsync(int id)
        {
            var sinistre = await _context.Sinistres.FindAsync(id);
            if (sinistre != null)
            {
                _context.Sinistres.Remove(sinistre);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SinistreExists(int id)
        {
            return await _context.Sinistres.AnyAsync(s => s.Id == id);
        }
    }
}

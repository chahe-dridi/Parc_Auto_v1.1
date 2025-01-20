using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class VisiteTechniqueService : IVisiteTechniqueService
    {
        private readonly ApplicationDbContext _context;

        public VisiteTechniqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VisiteTechnique>> GetAllVisiteTechniquesAsync()
        {
            return await _context.VisiteTechniques.Include(vt => vt.Voiture).ToListAsync();
        }

        public async Task<VisiteTechnique> GetVisiteTechniqueByIdAsync(int id)
        {
            return await _context.VisiteTechniques.Include(vt => vt.Voiture).FirstOrDefaultAsync(vt => vt.Id == id);
        }

        public async Task AddVisiteTechniqueAsync(VisiteTechnique visiteTechnique)
        {
            _context.Add(visiteTechnique);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVisiteTechniqueAsync(VisiteTechnique visiteTechnique)
        {
            _context.Update(visiteTechnique);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisiteTechniqueAsync(int id)
        {
            var visiteTechnique = await _context.VisiteTechniques.FindAsync(id);
            _context.VisiteTechniques.Remove(visiteTechnique);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VisiteTechniqueExistsAsync(int id)
        {
            return await _context.VisiteTechniques.AnyAsync(vt => vt.Id == id);
        }
    }
}

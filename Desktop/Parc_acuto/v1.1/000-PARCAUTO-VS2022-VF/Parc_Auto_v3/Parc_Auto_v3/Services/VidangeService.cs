using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class VidangeService : IVidangeService
    {
        private readonly ApplicationDbContext _context;

        public VidangeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vidange>> GetAllVidangesAsync()
        {
            return await _context.Vidanges.Include(v => v.Voiture).ToListAsync();
        }

        public async Task<Vidange> GetVidangeByIdAsync(int id)
        {
            return await _context.Vidanges.Include(v => v.Voiture).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddVidangeAsync(Vidange vidange)
        {
            _context.Add(vidange);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVidangeAsync(Vidange vidange)
        {
            _context.Update(vidange);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVidangeAsync(int id)
        {
            var vidange = await _context.Vidanges.FindAsync(id);
            if (vidange != null)
            {
                _context.Vidanges.Remove(vidange);
                await _context.SaveChangesAsync();
            }
        }
    }
}

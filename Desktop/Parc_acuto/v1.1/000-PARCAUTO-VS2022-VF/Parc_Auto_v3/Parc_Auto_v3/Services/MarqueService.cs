/*using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class MarqueService : IMarqueService
    {
        private readonly ApplicationDbContext _context;

        public MarqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marque>> GetAllMarquesAsync()
        {
            return await _context.Marques.ToListAsync();
        }

        public async Task<Marque> GetMarqueByIdAsync(int id)
        {
            return await _context.Marques.FindAsync(id);
        }

        public async Task AddMarqueAsync(Marque marque)
        {
            _context.Marques.Add(marque);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMarqueAsync(Marque marque)
        {
            _context.Entry(marque).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMarqueAsync(int marqueId)
        {
            var marque = await _context.Marques.FindAsync(marqueId);
            if (marque != null)
            {
                _context.Marques.Remove(marque);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Marque not found");
            }
        }

        public async Task DeleteModeleAsync(int modeleId)
        {
            var modele = await _context.Modeles.FindAsync(modeleId);
            if (modele != null)
            {
                _context.Modeles.Remove(modele);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Modele not found");
            }
        }








    }
}


*/




using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class MarqueService : IMarqueService
    {
        private readonly ApplicationDbContext _context;

        public MarqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Marque>> GetAllMarquesAsync()
        {
            return await _context.Marques.ToListAsync();
        }

        public async Task<Marque> GetMarqueByIdAsync(int id)
        {
            return await _context.Marques.FindAsync(id);
        }

        public async Task AddMarqueAsync(Marque marque)
        {
            _context.Marques.Add(marque);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMarqueAsync(Marque marque)
        {
            _context.Entry(marque).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMarqueAsync(int marqueId)
        {
            var marque = await _context.Marques.FindAsync(marqueId);
            if (marque != null)
            {
                _context.Marques.Remove(marque);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Marque not found");
            }
        }
    }
}

/*using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Services
{
    public class ModeleService : IModeleService
    {
        private readonly ApplicationDbContext _context;

        public ModeleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Modele>> GetAllModelesAsync()
        {
            return await _context.Modeles.ToListAsync();
        }

        public async Task<Modele> GetModeleByIdAsync(int id)
        {
            return await _context.Modeles.FindAsync(id);
        }


        public async Task<List<Modele>> GetModelesByMarqueIdAsync(int marqueId)
        {
            return await _context.Modeles.Where(m => m.MarqueId == marqueId).ToListAsync();
        }

     




        public async Task AddModeleAsync(Modele modele)
        {
            _context.Modeles.Add(modele);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModeleAsync(Modele modele)
        {
            _context.Entry(modele).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

		public async Task DeleteModeleAsync(int modeleId)
		{
			var modele = await _context.Modeles.FindAsync(modeleId);
			if (modele != null)
			{
				_context.Modeles.Remove(modele);
				await _context.SaveChangesAsync();
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
    public class ModeleService : IModeleService
    {
        private readonly ApplicationDbContext _context;

        public ModeleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Modele>> GetAllModelesAsync()
        {
            return await _context.Modeles.ToListAsync();
        }

        public async Task<Modele> GetModeleByIdAsync(int id)
        {
            return await _context.Modeles.FindAsync(id);
        }

        public async Task<List<Modele>> GetModelesByMarqueIdAsync(int marqueId)
        {
            return await _context.Modeles.Where(m => m.MarqueId == marqueId).ToListAsync();
        }

        public async Task AddModeleAsync(Modele modele)
        {
            _context.Modeles.Add(modele);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModeleAsync(Modele modele)
        {
            _context.Entry(modele).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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

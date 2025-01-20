using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
 
using System.Collections.Generic;
using System.Threading.Tasks;

public class VignetteService : IVignetteService
{
    private readonly ApplicationDbContext _context;

    public VignetteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vignette>> GetAllVignettesAsync()
    {
        return await _context.Vignettes.Include(v => v.Voiture).ToListAsync();
    }

    public async Task<Vignette> GetVignetteByIdAsync(int id)
    {
        return await _context.Vignettes.Include(v => v.Voiture).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddVignetteAsync(Vignette vignette)
    {
        _context.Add(vignette);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVignetteAsync(Vignette vignette)
    {
        _context.Update(vignette);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVignetteAsync(int id)
    {
        var vignette = await _context.Vignettes.FindAsync(id);
        _context.Vignettes.Remove(vignette);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> VignetteExistsAsync(int id)
    {
        return await _context.Vignettes.AnyAsync(e => e.Id == id);
    }
}

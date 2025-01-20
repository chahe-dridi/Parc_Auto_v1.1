using Parc_Auto_v3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IVignetteService
{
    Task<IEnumerable<Vignette>> GetAllVignettesAsync();
    Task<Vignette> GetVignetteByIdAsync(int id);
    Task AddVignetteAsync(Vignette vignette);
    Task UpdateVignetteAsync(Vignette vignette);
    Task DeleteVignetteAsync(int id);
    Task<bool> VignetteExistsAsync(int id);


 
 
}

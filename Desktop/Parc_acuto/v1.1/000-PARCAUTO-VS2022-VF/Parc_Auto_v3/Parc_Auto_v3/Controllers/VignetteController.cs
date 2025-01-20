using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "admin,superadmin")]
 
    public class VignetteController : Controller
    {
        private readonly IVignetteService _vignetteService;
        private readonly IVoitureService _voitureService;

        public VignetteController(IVignetteService vignetteService, IVoitureService voitureService)
        {
            _vignetteService = vignetteService;
            _voitureService = voitureService;
        }

        // GET: Vignette
        public async Task<IActionResult> Index()
        {
            var vignettes = await _vignetteService.GetAllVignettesAsync();
            return View(vignettes);
        }
 

        // GET: Vignette/Details/5
 
 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vignette = await _vignetteService.GetVignetteByIdAsync(id.Value);
            if (vignette == null)
            {
                return NotFound();
            }

            return View(vignette);
        }

 
 
        // GET: Vignette/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? voitureId)
        {
            var model = new Vignette
            {
                VoitureId = voitureId ?? 0 // Set the VoitureId in the model
            };

            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", voitureId);

            return View(model);
        }

        // POST: Vignette/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateEchance,DateValide,Alert,PrixUnitaire,VoitureId")] Vignette vignette)
        {
          //  if (ModelState.IsValid)
            {
                await _vignetteService.AddVignetteAsync(vignette);
                TempData["SuccessMessage"] = "Vignette has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vignette.VoitureId);
            return View(vignette);
        }

        // GET: Vignette/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vignette = await _vignetteService.GetVignetteByIdAsync(id.Value);
            if (vignette == null)
            {
                return NotFound();
            }
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vignette.VoitureId);
            return View(vignette);
        }

        // POST: Vignette/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateEchance,DateValide,Alert,PrixUnitaire,VoitureId")] Vignette vignette)
        {
            if (id != vignette.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    await _vignetteService.UpdateVignetteAsync(vignette);
                    TempData["SuccessMessage"] = "Vignette has been updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VignetteExists(vignette.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vignette.VoitureId);
            return View(vignette);
        }

        // GET: Vignette/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vignette = await _vignetteService.GetVignetteByIdAsync(id.Value);
            if (vignette == null)
            {
                return NotFound();
            }

            return View(vignette);
        }

        // POST: Vignette/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vignetteService.DeleteVignetteAsync(id);
            TempData["SuccessMessage"] = "Vignette has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VignetteExists(int id)
        {
            var vignette = await _vignetteService.GetVignetteByIdAsync(id);
            return vignette != null;
        }


         

        public async Task<IActionResult> Search(string searchString)
        {
            var vignettes = await _vignetteService.GetAllVignettesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                vignettes = vignettes
                    .Where(v => v.Voiture.Matricule.Contains(searchString))
                    .ToList();
            }

            return PartialView("_vignetteListPartial", vignettes);
        }


 

    }
}

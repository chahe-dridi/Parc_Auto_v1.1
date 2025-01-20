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
 
    public class SinistreController : Controller
    {
        private readonly ISinistreService _sinistreService;
        private readonly ApplicationDbContext _context;

        public SinistreController(ISinistreService sinistreService, ApplicationDbContext context)
        {
            _sinistreService = sinistreService;
            _context = context;
        }

        // GET: Sinistre
        public async Task<IActionResult> Index()
        {
            var sinistres = await _sinistreService.GetAllSinistresAsync();
            return View(sinistres);
        }

        // GET: Sinistre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinistre = await _sinistreService.GetSinistreByIdAsync(id.Value);
            if (sinistre == null)
            {
                return NotFound();
            }

            return View(sinistre);
        }
 
        // GET: Sinistre/Create
        public IActionResult Create(int? voitureId)
        {
            ViewData["VoitureId"] = new SelectList(_context.Voitures, "Id", "Matricule", voitureId);
            return View();
        }


        // POST: Sinistre/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateDommage,Observation,Description,EtatDeVoiture,PrixFixe,Date_circulation,VoitureId")] Sinistre sinistre)
        {
            //if (ModelState.IsValid)
            {
                await _sinistreService.AddSinistreAsync(sinistre);
                TempData["SuccessMessage"] = "Sinistre has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["VoitureId"] = new SelectList(_context.Voitures, "Id", "Matricule", sinistre.VoitureId);
            return View(sinistre);
        }
 
        // GET: Sinistre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinistre = await _sinistreService.GetSinistreByIdAsync(id.Value);
            if (sinistre == null)
            {
                return NotFound();
            }
            ViewData["VoitureId"] = new SelectList(_context.Voitures, "Id", "Matricule", sinistre.VoitureId);
            return View(sinistre);
        }

        // POST: Sinistre/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateDommage,Observation,Description,Date_circulation,EtatDeVoiture,PrixFixe,VoitureId")] Sinistre sinistre)
        {
            if (id != sinistre.Id)
            {
                return NotFound();
            }

          //  if (ModelState.IsValid)
            {
                try
                {
                    await _sinistreService.UpdateSinistreAsync(sinistre);
                    TempData["SuccessMessage"] = "Sinistre has been updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SinistreExists(sinistre.Id))
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
            ViewData["VoitureId"] = new SelectList(_context.Voitures, "Id", "Matricule", sinistre.VoitureId);
            return View(sinistre);
        }

        // GET: Sinistre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sinistre = await _sinistreService.GetSinistreByIdAsync(id.Value);
            if (sinistre == null)
            {
                return NotFound();
            }

            return View(sinistre);
        }

        // POST: Sinistre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sinistreService.DeleteSinistreAsync(id);
            TempData["SuccessMessage"] = "Sinistre has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SinistreExists(int id)
        {
            return await _sinistreService.SinistreExists(id);
        }





        public async Task<IActionResult> Search(string searchString)
        {
            var sinistres = await _sinistreService.GetAllSinistresAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                sinistres= sinistres  
                    .Where(v => v.Voiture.Matricule.Contains(searchString))
                    .ToList();
            }

            return PartialView("_SinistreListPartial", sinistres);
        }
 
  

    }
}

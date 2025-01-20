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
 
    public class VidangeController : Controller
    {
        private readonly IVidangeService _vidangeService;
        private readonly IVoitureService _voitureService;

        public VidangeController(IVidangeService vidangeService, IVoitureService voitureService)
        {
            _vidangeService = vidangeService;
            _voitureService = voitureService;
        }

        // GET: Vidange
        public async Task<IActionResult> Index()
        {
            var vidanges = await _vidangeService.GetAllVidangesAsync();
            return View(vidanges);
        }

        // GET: Vidange/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidange = await _vidangeService.GetVidangeByIdAsync(id.Value);
            if (vidange == null)
            {
                return NotFound();
            }

            return View(vidange);
        }
 
        // GET: Vidange/Create
        public async Task<IActionResult> Create(int? voitureId)
        {
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", voitureId);
            return View();
        }
 
        // POST: Vidange/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateIntervention,TypeIntervention,Fournisseur,NFacture,Kilometrage,OperationDetails,MontantHt,VoitureId")] Vidange vidange)
        {
          //  if (ModelState.IsValid)
            {
                await _vidangeService.AddVidangeAsync(vidange);
                TempData["SuccessMessage"] = "Vidange has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vidange.VoitureId);
            return View(vidange);
        }


        // GET: Vidange/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidange = await _vidangeService.GetVidangeByIdAsync(id.Value);
            if (vidange == null)
            {
                return NotFound();
            }
            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vidange.VoitureId);
            return View(vidange);
        }

        // POST: Vidange/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateIntervention,TypeIntervention,Fournisseur,NFacture,Kilometrage,OperationDetails,MontantHt,VoitureId")] Vidange vidange)
        {
            if (id != vidange.Id)
            {
                return NotFound();
            }

          //  if (ModelState.IsValid)
            {
                try
                {
                    await _vidangeService.UpdateVidangeAsync(vidange);
                    TempData["SuccessMessage"] = "Vidange has been updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VidangeExists(vidange.Id))
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
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", vidange.VoitureId);
            return View(vidange);
        }

        // GET: Vidange/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vidange = await _vidangeService.GetVidangeByIdAsync(id.Value);
            if (vidange == null)
            {
                return NotFound();
            }

            return View(vidange);
        }

        // POST: Vidange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vidangeService.DeleteVidangeAsync(id);
            TempData["SuccessMessage"] = "Vidange has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VidangeExists(int id)
        {
            var vidange = await _vidangeService.GetVidangeByIdAsync(id);
            return vidange != null;
        }
 
        public async Task<IActionResult> Search(string searchString)
        {
            var vidanges = await _vidangeService.GetAllVidangesAsync();
            
            if (!string.IsNullOrEmpty(searchString))
            {
                vidanges = vidanges
                    .Where(v => v.Voiture.Matricule.Contains(searchString))
                    .ToList();
            }

            return PartialView("_VidangeListPartial", vidanges);
        }

 

    }
}

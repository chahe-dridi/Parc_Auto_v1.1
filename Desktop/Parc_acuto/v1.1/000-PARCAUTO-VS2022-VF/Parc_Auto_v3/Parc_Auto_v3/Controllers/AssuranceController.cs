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

    public class AssuranceController : Controller
    {
        private readonly IAssuranceService _assuranceService;
        private readonly IVoitureService _voitureService;

        public AssuranceController(IAssuranceService assuranceService, IVoitureService voitureService)
        {
            _assuranceService = assuranceService;
            _voitureService = voitureService;
        }

        // GET: Assurance
        public async Task<IActionResult> Index()
        {
            var assurances = await _assuranceService.GetAllAssurancesAsync();
            return View(assurances);
        }

        // GET: Assurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assurance = await _assuranceService.GetAssuranceByIdAsync(id.Value);
            if (assurance == null)
            {
                return NotFound();
            }

            return View(assurance);
        }

        // GET: Assurance/Create/{voitureId}
        public async Task<IActionResult> Create(int voitureId)
        {
            var model = new Assurance
            {
                VoitureId = voitureId
            };

            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", voitureId);
            return View(model);
        }

        // POST: Assurance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateEchance,DateValide,Alert,PrixUnitaire,VoitureId")] Assurance assurance)
        {
            //if (ModelState.IsValid)
            {
                await _assuranceService.AddAssuranceAsync(assurance);
                TempData["SuccessMessage"] = "Assurance has been created successfully!";
                return RedirectToAction(nameof(Index));
            }

            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewData["VoitureId"] = new SelectList(voitures, "Id", "Matricule", assurance.VoitureId);
            return View(assurance);
        }



 





 


        // GET: Assurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assurance = await _assuranceService.GetAssuranceByIdAsync(id.Value);
            if (assurance == null)
            {
                return NotFound();
            }

            var voitures = await _voitureService.GetAllVoituresAsync();
            if (voitures == null)
            {
                // Handle the case where voitures is null
                ViewBag.Voitures = new SelectList(Enumerable.Empty<SelectListItem>(), "Id", "Matricule");
            }
            else
            {
                ViewBag.Voitures = new SelectList(voitures, "Id", "Matricule", assurance.VoitureId);
            }

            return View(assurance);
        }


        // POST: Assurance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateEchance,DateValide,Alert,PrixUnitaire,VoitureId")] Assurance assurance)
        {
            if (id != assurance.Id)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            {
                try
                {
                    await _assuranceService.UpdateAssuranceAsync(assurance);
                    TempData["SuccessMessage"] = "Assurance has been updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AssuranceExists(assurance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var voitures = await _voitureService.GetAllVoituresAsync();
            ViewBag.Voitures = new SelectList(voitures, "Id", "Matricule", assurance.VoitureId);
            return View(assurance);
        }


















        private async Task<bool> AssuranceExists(int id)
        {
            var assurance = await _assuranceService.GetAssuranceByIdAsync(id);
            return assurance != null;
        }


        
        // GET: Assurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var assurance = await _assuranceService.GetAssuranceByIdAsync(id.Value);
            if (assurance == null)
            {
                return NotFound();
            }

            return View(assurance);
        }

        // POST: Assurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _assuranceService.DeleteAssuranceAsync(id);
            TempData["SuccessMessage"] = "Assurance has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Search(string searchString)
        {
            var assurances = await _assuranceService.GetAllAssurancesAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                assurances = assurances
                    .Where(v => v.Voiture.Matricule.Contains(searchString))
                    .ToList();
            }

            return PartialView("_AssuranceListPartial", assurances);
        }










    }

 
   
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
 
using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Linq;
using System.Threading.Tasks;


using System.Collections.Generic;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace Parc_Auto_v3.Controllers  
{
 
    [Authorize(Roles= "admin,superadmin")]
 
 
    public class VoitureController : Controller
    {
        private readonly IVoitureService _voitureService;
        private readonly IMarqueService _marqueService;
        private readonly IModeleService _modeleService;

        //
        private readonly IDemandesService _demandesService;

        public VoitureController(IVoitureService voitureService, IMarqueService marqueService, IModeleService modeleService   , IDemandesService demandesService)
        
        {
            _voitureService = voitureService;
            _marqueService = marqueService;
            _modeleService = modeleService;
            //
            _demandesService = demandesService;

        }

        // GET: Voiture
       public async Task<IActionResult> Index()
          {
              var voitures = await _voitureService.GetAllVoituresAsync1();
              return View(voitures);
          }  
       

        // GET: Voiture/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _voitureService.GetVoitureByIdAsync1(id.Value);
            if (voiture == null)
            {
                return NotFound();
            }

            var totalVisiteTechnique = await _voitureService.GetTotalVisiteTechniqueCostAsync(id.Value);
            var totalVignette = await _voitureService.GetTotalVignetteCostAsync(id.Value);
            var totalVidange = await _voitureService.GetTotalVidangeCostAsync(id.Value);
            var totalAssurance = await _voitureService.GetTotalAssuranceCostAsync(id.Value);

            var demandes = await _voitureService.GetDemandesByVoitureIdAsync(id.Value);

            ViewBag.TotalVisiteTechnique = totalVisiteTechnique;
            ViewBag.TotalVignette = totalVignette;
            ViewBag.TotalVidange = totalVidange;
            ViewBag.TotalAssurance = totalAssurance;
            ViewBag.Demandes = demandes;

            return View(voiture);
        }






        //
      /*  public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _voitureService.GetVoitureByIdAsync(id.Value);
            if (voiture == null)
            {
                return NotFound();
            }

            var totalVisiteTechnique = await _voitureService.GetTotalVisiteTechniqueCostAsync(id.Value);
            var totalVignette = await _voitureService.GetTotalVignetteCostAsync(id.Value);
            var totalVidange = await _voitureService.GetTotalVidangeCostAsync(id.Value);
            var totalAssurance = await _voitureService.GetTotalAssuranceCostAsync(id.Value);

            var demandes = await _voitureService.GetDemandesByVoitureIdAsync(id.Value);

            ViewBag.TotalVisiteTechnique = totalVisiteTechnique;
            ViewBag.TotalVignette = totalVignette;
            ViewBag.TotalVidange = totalVidange;
            ViewBag.TotalAssurance = totalAssurance;
            ViewBag.Demandes = demandes;

            return View(voiture);
        }
      */
         


        // GET: Voiture/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Marques = await _marqueService.GetAllMarquesAsync();
            return View();
        }

        // POST: Voiture/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Matricule,NumeroSerieCarteGrise,TypeVoiture,MarqueId,ModeleId,Km,Carburant,Etat,Disponibilite")] Voiture voiture)
        {
         //   if (ModelState.IsValid)
            {
                await _voitureService.AddVoitureAsync(voiture);
                TempData["SuccessMessage"] = "Voiture has been added successfully!";
                return RedirectToAction(nameof(Index));
            }
           ViewBag.Marques = await _marqueService.GetAllMarquesAsync();
            return View(voiture);
        }

      

        // GET: Voiture/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _voitureService.GetVoitureByIdAsync(id.Value);
            if (voiture == null)
            {
                return NotFound();
            }

            ViewBag.Marques = await _marqueService.GetAllMarquesAsync();
            ViewBag.Modeles = await _modeleService.GetModelesByMarqueIdAsync(voiture.MarqueId);

            return View(voiture);
        }



        // POST: Voiture/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Matricule,NumeroSerieCarteGrise,TypeVoiture,MarqueId,ModeleId,Km,Carburant,Etat,Disponibilite")] Voiture voiture)
        {
            if (id != voiture.Id)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
            {
                try
                {
                    await _voitureService.UpdateVoitureAsync(voiture);
                    TempData["SuccessMessage"] = "Voiture has been updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VoitureExists(voiture.Id))
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
            ViewBag.Marques = await _marqueService.GetAllMarquesAsync();
            ViewBag.Modeles = await _modeleService.GetAllModelesAsync();
            return View(voiture);
        }

        // GET: Voiture/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voiture = await _voitureService.GetVoitureByIdAsync(id.Value);
            if (voiture == null)
            {
                return NotFound();
            }

            return View(voiture);
        }

        // POST: Voiture/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _voitureService.DeleteVoitureAsync(id);
            TempData["SuccessMessage"] = "Voiture has been deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VoitureExists(int id)
        {
            var voiture = await _voitureService.GetVoitureByIdAsync(id);
            return voiture != null;
        }

     
      

 

        [HttpGet]
        public async Task<IActionResult> Search(string searchString)
        {
            var voitures = await _voitureService.GetAllVoituresAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                voitures = voitures.Where(v => v.Matricule.Contains(searchString)).ToList();
            }

            return PartialView("_VoitureListPartial", voitures);
        }













        [HttpPost]
        public async Task<IActionResult> AddMarque(string newMarque)
        {
            if (!string.IsNullOrWhiteSpace(newMarque))
            {
                await _marqueService.AddMarqueAsync(new Marque { Nom = newMarque });
                return Ok();
            }
            return BadRequest("Invalid marque name");
        }

        [HttpPost]
        public async Task<IActionResult> AddModele(string newModele, int marqueId)
        {
            if (!string.IsNullOrWhiteSpace(newModele))
            {
                await _modeleService.AddModeleAsync(new Modele { Nom = newModele, MarqueId = marqueId });
                return Ok();
            }
            return BadRequest("Invalid modele name or marque ID");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMarque(int marqueId, string updatedMarque)
        {
            if (marqueId > 0 && !string.IsNullOrWhiteSpace(updatedMarque))
            {
                var marque = await _marqueService.GetMarqueByIdAsync(marqueId);
                if (marque != null)
                {
                    marque.Nom = updatedMarque;
                    await _marqueService.UpdateMarqueAsync(marque);
                    return Ok();
                }
            }
            return BadRequest("Invalid marque or ID");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateModele(int modeleId, string updatedModele)
        {
            if (modeleId > 0 && !string.IsNullOrWhiteSpace(updatedModele))
            {
                var modele = await _modeleService.GetModeleByIdAsync(modeleId);
                if (modele != null)
                {
                    modele.Nom = updatedModele;
                    await _modeleService.UpdateModeleAsync(modele);
                    return Ok();
                }
            }
            return BadRequest("Invalid modele or ID");
        }

    



        [HttpPost]
        public async Task<IActionResult> DeleteMarque(int marqueId)
        {
            try
            {
                await _marqueService.DeleteMarqueAsync(marqueId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModele(int modeleId)
        {
            try
            {
                await _modeleService.DeleteModeleAsync(modeleId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }










        // GET: Voiture/GetModelesByMarqueId
        [HttpGet]
        public async Task<IActionResult> GetModelesByMarqueId(int marqueId)
        {
            if (marqueId > 0)
            {
                var modeles = await _modeleService.GetModelesByMarqueIdAsync(marqueId);
                return Json(modeles);
            }

            return BadRequest();
        }






        public async Task<IActionResult> DownloadExcel()
        {
            var voitures = await _voitureService.GetAllVoituresAsync1();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Voitures Report");

                // Set headers
                var headers = new[]
                {
                    "Matricule", "Type Voiture", "Marque", "Modele", "Carburant", "Numero Serie Carte Grise", "Disponibilite", "Kilométrage"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    worksheet.Cells[1, i + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[1, i + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                // Populate data
                for (int row = 0; row < voitures.Count; row++)
                {
                    var voiture = voitures[row];
                    var lastKilometrage = voiture.Demandes?.OrderByDescending(d => d.DateDepart).FirstOrDefault()?.Kilometrage;

                    worksheet.Cells[row + 2, 1].Value = voiture.Matricule;
                    worksheet.Cells[row + 2, 2].Value = voiture.TypeVoiture;
                    worksheet.Cells[row + 2, 3].Value = voiture.Marque.Nom;
                    worksheet.Cells[row + 2, 4].Value = voiture.Modele.Nom;
                    worksheet.Cells[row + 2, 5].Value = voiture.Carburant;
                    worksheet.Cells[row + 2, 6].Value = voiture.NumeroSerieCarteGrise;
                    worksheet.Cells[row + 2, 7].Value = voiture.Disponibilite;
                    worksheet.Cells[row + 2, 8].Value = lastKilometrage;
                }

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    var fileBytes = stream.ToArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Voitures.xlsx");
                }
            }
        }










    }
}

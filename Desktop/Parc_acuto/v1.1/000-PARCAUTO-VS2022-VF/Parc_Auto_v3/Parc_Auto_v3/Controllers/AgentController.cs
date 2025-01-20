/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "agent,superadmin")]
    public class AgentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDemandesService _demandesService;
        private readonly IVoitureService _voitureService;

        public AgentController(ApplicationDbContext context, IDemandesService demandesService, IVoitureService voitureService)
        {
            _context = context;
            _demandesService = demandesService;
            _voitureService = voitureService;
        }

        // GET: Agent/Index
    public async Task<IActionResult> Index()
   {
       var demandes = await _demandesService.GetAllDemandesAsync();
       // Filter demandes to include only those without kilometrage and with a reserved voiture
       var filteredDemandes = demandes
           .Where(d => d.Kilometrage == null && d.Voiture != null && d.Voiture.Disponibilite == "Reserved")
           .ToList();

       return View(filteredDemandes);
   }

    // POST: Agent/UpdateDemande
     [HttpPost]
     public async Task<IActionResult> UpdateDemande(int id, int kilometrage)
     {
         var demande = await _demandesService.GetDemandeByIdAsync(id);
         if (demande == null)
         {
             return NotFound();
         }

         // Update the kilometrage and save
         demande.Kilometrage = kilometrage;
         await _demandesService.UpdateDemandeAsync(demande);

         // Make the associated voiture available
         if (demande.VoitureId.HasValue)
         {
             var voiture = await _voitureService.GetVoitureByIdAsync(demande.VoitureId.Value);
             if (voiture != null)
             {
                 voiture.Disponibilite = "Disponible";
                 await _voitureService.UpdateVoitureAsync(voiture);
             }
         }

         return RedirectToAction(nameof(Index));
     }















}
}
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "agent,superadmin")]
    public class AgentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDemandesService _demandesService;
        private readonly IVoitureService _voitureService;

        public AgentController(ApplicationDbContext context, IDemandesService demandesService, IVoitureService voitureService)
        {
            _context = context;
            _demandesService = demandesService;
            _voitureService = voitureService;
        }

        // GET: Agent/Index
        public async Task<IActionResult> Index()
        {
            var demandes = await _demandesService.GetAllDemandesAsync();
            // Filter demandes to include only those without kilometrage and with a reserved voiture
            var filteredDemandes = demandes
                .Where(d => d.Kilometrage == null && d.Voiture != null && d.Voiture.Disponibilite == "Reserved")
                .ToList();

            var model = new List<DemandeViewModel>();

            foreach (var demande in filteredDemandes)
            {
                var previousDemande = await _context.Demandes
                    .Where(d => d.VoitureId == demande.VoitureId && d.Id < demande.Id)
                    .OrderByDescending(d => d.Id)
                    .FirstOrDefaultAsync();

                model.Add(new DemandeViewModel
                {
                    Demande = demande,
                    LastKilometrage = previousDemande?.Kilometrage
                });
            }

            return View(model);
        }

        // POST: Agent/UpdateDemande
        [HttpPost]
        public async Task<IActionResult> UpdateDemande(int id, int newKilometrage)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }

            // Update the kilometrage and save
            demande.Kilometrage = newKilometrage;
            await _demandesService.UpdateDemandeAsync(demande);

            // Make the associated voiture available
            if (demande.VoitureId.HasValue)
            {
                var voiture = await _voitureService.GetVoitureByIdAsync(demande.VoitureId.Value);
                if (voiture != null)
                {
                    voiture.Disponibilite = "Disponible";
                    await _voitureService.UpdateVoitureAsync(voiture);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class DemandeViewModel
    {
        public Demandes Demande { get; set; }
        public int? LastKilometrage { get; set; }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "utilisateur,admin,superadmin")]
    
 
    public class DemandesController : Controller
    {
        private readonly IDemandesService _demandesService;
        private readonly UserManager<ApplicationUser> _userManager;
        public DemandesController(IDemandesService demandesService, UserManager<ApplicationUser> userManager)
        {
            _demandesService = demandesService;
            _userManager = userManager;
        }

       

        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized(); // Redirect to login if the user is not logged in
            }

            // Check if the user is in the "admin" role
            // bool isAdmin = await _userManager.IsInRoleAsync(user, "admin,superadmin");

            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "superadmin");

            bool isAdminOrSuperAdmin = isAdmin || isSuperAdmin;



            IEnumerable<Demandes> demandes;

            // if (isAdmin)
            if (isAdminOrSuperAdmin)
            {
                // If the user is an admin, show all demandes
                demandes = await _demandesService.GetAllDemandesAsync();
            }
            else
            {
                // If the user is not an admin, show only their demandes
                demandes = await _demandesService.GetDemandesByUserIdAsync(user.Id);
            }

            // Sorting by Id in descending order
            var sortedDemandes = demandes.OrderByDescending(d => d.Id);

            return View(sortedDemandes);
        }





        public async Task<IActionResult> Details(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
            return View(demande);
        }

        public IActionResult Create()
        {
            return View();
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Demandes demande)
        {
           // if (ModelState.IsValid)
            {
                // Get the current logged-in user
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    demande.UserId = user.Id; // Set the UserId to the current user's Id
                    demande.Etat = "En attente"; // Set Etat to "En attente"
                    await _demandesService.AddDemandeAsync(demande);
                    return RedirectToAction(nameof(Index));
                }
        
               
            }

            return View(demande);
        }



 



        public async Task<IActionResult> Edit(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
            return View(demande);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Demandes demande)
        {
            if (id != demande.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)

            {
               if (demande.Accompagnateur == null) { demande.Accompagnateur = " "; }
                await _demandesService.UpdateDemandeAsync(demande);
                return RedirectToAction(nameof(Index));
            }
            return View(demande);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var demande = await _demandesService.GetDemandeByIdAsync(id);
            if (demande == null)
            {
                return NotFound();
            }
            return View(demande);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _demandesService.DeleteDemandeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

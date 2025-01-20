using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parc_Auto_v3.Models;
using Parc_Auto_v3.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parc_Auto_v3.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<AdminController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Email.Contains(searchString) || u.UserName.Contains(searchString));
            }

            var userList = await users.ToListAsync();
            var model = new List<UserIndexMVW>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserIndexMVW
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    CurrentRole = roles.FirstOrDefault() // Assuming a user has only one role
                });
            }

            return PartialView("_UsersListPartial", model);
       
        }










        public async Task<IActionResult> Index()
        {
            var users = _context.Users.ToList();
            var model = new List<UserIndexMVW>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserIndexMVW
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    CurrentRole = roles.FirstOrDefault() // Assuming a user has only one role
                });
            }

            return View(model);
        }
       
        

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userName, string newRole)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return View("Index", await GetUsersWithRoles());
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user role");
                return View("Index", await GetUsersWithRoles());
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<UserIndexMVW>> GetUsersWithRoles()
        {
            var users = _context.Users.ToList();
            var model = new List<UserIndexMVW>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserIndexMVW
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    CurrentRole = roles.FirstOrDefault() // Assuming a user has only one role
                });
            }

            return model;
        }
    }
}

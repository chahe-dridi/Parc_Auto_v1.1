using Microsoft.AspNetCore.Mvc;
using Parc_Auto_v3.Models;
using System.Diagnostics;
using Parc_Auto_v3.Services;
namespace Parc_Auto_v3.Controllers
{
       public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Test database connection
                var isConnected = await _context.Database.CanConnectAsync();
                _logger.LogInformation($"Database connection established: {isConnected}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error connecting to database: {ex.Message}");
            }

            ViewData["Title"] = "Hello Page";
            return View();
        }



        public IActionResult hello()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    

     

     

      
    }
}

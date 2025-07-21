using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ForkliftHub.Controllers
{
    [AllowAnonymous]
    public class HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ApplicationDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.MachineModel)
                .OrderByDescending(p => p.Id) // Latest added
                .Take(6)
                .ToListAsync();

            var vm = new ProductListViewModel
            {
                Products = products
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

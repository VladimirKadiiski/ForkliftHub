using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ForkliftHub.Controllers
{
    [AllowAnonymous]
    public class HomeController(ILogger<HomeController> logger, IHomeService homeService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IHomeService _homeService = homeService;

        public async Task<IActionResult> Index()
        {
            var vm = await _homeService.GetHomePageProductsAsync();
            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

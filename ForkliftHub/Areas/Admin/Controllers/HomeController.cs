using ForkliftHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController(IAdminDashboardService dashboardService) : Controller
    {
        private readonly IAdminDashboardService _dashboardService = dashboardService;

        public async Task<IActionResult> Index()
        {
            var vm = await _dashboardService.GetDashboardDataAsync();
            return View(vm);
        }
    }
}
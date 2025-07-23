using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftHub.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class HomeController(IUserDashboardService dashboardService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IUserDashboardService _dashboardService = dashboardService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = await _dashboardService.GetDashboardDataAsync(user.Id);
            return View(vm);
        }
    }
}
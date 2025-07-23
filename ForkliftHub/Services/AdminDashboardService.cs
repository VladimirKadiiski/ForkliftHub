using ForkliftHub.Data;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class AdminDashboardService(ApplicationDbContext context) : IAdminDashboardService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
        {
            return new AdminDashboardViewModel
            {
                ProductCount = await _context.Products.CountAsync(),
                UserCount = await _context.Users.CountAsync(),
                ReviewCount = await _context.Reviews.CountAsync()
            };
        }
    }
}

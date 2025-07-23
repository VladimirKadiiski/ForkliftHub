using ForkliftHub.Data;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class UserDashboardService(ApplicationDbContext context) : IUserDashboardService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<UserDashboardViewModel> GetDashboardDataAsync(string userId)
        {
            return new UserDashboardViewModel
            {
                MyReviews = await _context.Reviews
                    .Include(r => r.Product)
                    .Where(r => r.UserId == userId)
                    .ToListAsync()
            };
        }
    }
}

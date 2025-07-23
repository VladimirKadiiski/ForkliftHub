using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IUserDashboardService
    {
        Task<UserDashboardViewModel> GetDashboardDataAsync(string userId);
    }
}

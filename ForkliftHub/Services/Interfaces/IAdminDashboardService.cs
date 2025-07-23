using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardViewModel> GetDashboardDataAsync();
    }
}

using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IHomeService
    {
        Task<ProductListViewModel> GetHomePageProductsAsync(int count = 6);
    }
}

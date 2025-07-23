using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandViewModel>> GetAllAsync();
        Task<BrandViewModel?> GetByIdAsync(int id);
        Task CreateAsync(BrandViewModel vm);
        Task<bool> UpdateAsync(BrandViewModel vm);
        Task<bool> DeleteAsync(int id);
    }
}

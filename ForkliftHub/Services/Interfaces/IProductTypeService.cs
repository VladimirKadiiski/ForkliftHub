using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeViewModel>> GetAllAsync();
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
        Task CreateAsync(ProductTypeViewModel vm);
        Task<ProductTypeViewModel?> GetByIdAsync(int id);
        Task UpdateAsync(ProductTypeViewModel vm);
        Task DeleteAsync(int id);
    }
}

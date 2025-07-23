using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
        Task<CategoryViewModel?> GetCategoryByIdAsync(int id);
        Task<bool> CategoryExistsAsync(string name, int? excludeId = null);
        Task CreateCategoryAsync(CategoryViewModel vm);
        Task UpdateCategoryAsync(CategoryViewModel vm);
        Task DeleteCategoryAsync(int id);
    }
}

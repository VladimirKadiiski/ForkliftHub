using ForkliftHub.Models;
using ForkliftHub.ViewModels;

namespace ForkliftHub.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductListViewModel> GetFilteredProductListAsync(
            string? searchTerm,
            int? categoryId,
            int? brandId,
            int? engineId,
            int? mastTypeId,
            string? sortOrder,
            int page,
            int pageSize);

        Task<Product?> GetProductDetailsAsync(int? id);
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);

        Task<ProductFormViewModel> GetProductFormViewModelAsync(int? id = null);
        Task CreateProductAsync(ProductFormViewModel viewModel);
        Task<bool> UpdateProductAsync(ProductFormViewModel viewModel);
    }
}

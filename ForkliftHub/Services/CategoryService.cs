using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class CategoryService(ApplicationDbContext context) : ICategoryService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }

        public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category == null ? null : new CategoryViewModel { Id = category.Id, Name = category.Name };
        }

        public async Task<bool> CategoryExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task CreateCategoryAsync(CategoryViewModel vm)
        {
            _context.Categories.Add(new Category { Name = vm.Name });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryViewModel vm)
        {
            var category = await _context.Categories.FindAsync(vm.Id);
            if (category == null) return;

            category.Name = vm.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}

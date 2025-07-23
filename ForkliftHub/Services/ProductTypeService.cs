using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class ProductTypeService(ApplicationDbContext context) : IProductTypeService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<ProductTypeViewModel>> GetAllAsync()
        {
            return await _context.ProductTypes
                .Select(pt => new ProductTypeViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.ProductTypes
                .AnyAsync(pt => pt.Name == name && (!excludeId.HasValue || pt.Id != excludeId.Value));
        }

        public async Task CreateAsync(ProductTypeViewModel vm)
        {
            var productType = new ProductType { Name = vm.Name };
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductTypeViewModel?> GetByIdAsync(int id)
        {
            var pt = await _context.ProductTypes.FindAsync(id);
            if (pt == null) return null;

            return new ProductTypeViewModel
            {
                Id = pt.Id,
                Name = pt.Name
            };
        }

        public async Task UpdateAsync(ProductTypeViewModel vm)
        {
            var pt = await _context.ProductTypes.FindAsync(vm.Id);
            if (pt == null) return;

            pt.Name = vm.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pt = await _context.ProductTypes.FindAsync(id);
            if (pt == null) return;

            _context.ProductTypes.Remove(pt);
            await _context.SaveChangesAsync();
        }
    }
}

using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class BrandService(ApplicationDbContext context) : IBrandService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<BrandViewModel>> GetAllAsync()
        {
            return await _context.Brands
                .Select(b => new BrandViewModel { Id = b.Id, Name = b.Name })
                .ToListAsync();
        }

        public async Task<BrandViewModel?> GetByIdAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            return brand == null ? null : new BrandViewModel { Id = brand.Id, Name = brand.Name };
        }

        public async Task CreateAsync(BrandViewModel vm)
        {
            var brand = new Brand { Name = vm.Name };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BrandViewModel vm)
        {
            var brand = await _context.Brands.FindAsync(vm.Id);
            if (brand == null) return false;

            brand.Name = vm.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

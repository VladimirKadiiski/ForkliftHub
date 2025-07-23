using ForkliftHub.Data;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class HomeService(ApplicationDbContext context) : IHomeService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ProductListViewModel> GetHomePageProductsAsync(int count = 6)
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.MachineModel)
                .OrderByDescending(p => p.Id)
                .Take(count)
                .ToListAsync();

            return new ProductListViewModel
            {
                Products = products
            };
        }
    }
}

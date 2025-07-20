using ForkliftHub.Data;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Areas.User.Controllers
{
    [Area("User")]
    public class ProductsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        private ProductFormViewModel PopulateDropdowns(ProductFormViewModel vm)
        {
            vm.Brands = new SelectList(_context.Brands, "Id", "Name", vm.BrandId);
            vm.MachineModels = new SelectList(_context.MachineModels, "Id", "Name", vm.MachineModelId);
            vm.Categories = new SelectList(_context.Categories, "Id", "Name", vm.CategoryId);
            vm.Engines = new SelectList(_context.Engines, "Id", "Type", vm.EngineId);
            vm.MastTypes = new SelectList(_context.MastTypes, "Id", "Name", vm.MastTypeId);
            vm.ProductTypes = new SelectList(_context.ProductTypes, "Id", "Name", vm.ProductTypeId);
            return vm;
        }

        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int? brandId, int? EngineId, int? MastTypeId, string? sortOrder, int page = 1)

        {
            const int pageSize = 10;

            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.MachineModel)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm) ||
                    p.Brand.Name.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (EngineId.HasValue)
            {
                query = query.Where(p => p.EngineId == EngineId.Value);
            }

            if (MastTypeId.HasValue)
            {
                query = query.Where(p => p.MastTypeId == MastTypeId.Value);
            }

            int totalCount = await query.CountAsync();


            if (!string.IsNullOrEmpty(sortOrder))
            {
                query = sortOrder switch
                {
                    "price_desc" => query.OrderByDescending(p => p.Price),
                    "price_asc" => query.OrderBy(p => p.Price),
                    _ => query.OrderBy(p => p.Name)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                SelectedBrandId = brandId,
                SelectedEngineId = EngineId,
                SelectedMastTypeId = MastTypeId,
                Categories = await _context.Categories
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToListAsync(),
                Brands = await _context.Brands
                    .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                    .ToListAsync(),
                Engines = await _context.Engines
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type })
                    .ToListAsync(),
                MastTypes = await _context.MastTypes
                    .Select(mt => new SelectListItem { Value = mt.Id.ToString(), Text = mt.Name })
                    .ToListAsync(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                SortOrder = sortOrder
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.MachineModel)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}

using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services.Interfaces;
using ForkliftHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Services
{
    public class ProductService(ApplicationDbContext context, IWebHostEnvironment environment) : IProductService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _environment = environment;
                
        public async Task<ProductFormViewModel> GetProductFormViewModelAsync(int? id = null)
        {
            var brands = await _context.Brands.OrderBy(b => b.Name)
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
                .ToListAsync();

            var categories = await _context.Categories.OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            var productTypes = await _context.ProductTypes.OrderBy(pt => pt.Name)
                .Select(pt => new SelectListItem { Value = pt.Id.ToString(), Text = pt.Name })
                .ToListAsync();

            var engines = await _context.Engines.OrderBy(e => e.Type)
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type })
                .ToListAsync();

            var mastTypes = await _context.MastTypes.OrderBy(m => m.Name)
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToListAsync();

            var machineModels = await _context.MachineModels.OrderBy(mm => mm.Name)
                .Select(mm => new SelectListItem { Value = mm.Id.ToString(), Text = mm.Name })
                .ToListAsync();

            if (id == null)
            {
                return new ProductFormViewModel
                {
                    Brands = new SelectList(brands, "Value", "Text"),
                    Categories = new SelectList(categories, "Value", "Text"),
                    ProductTypes = new SelectList(productTypes, "Value", "Text"),
                    Engines = new SelectList(engines, "Value", "Text"),
                    MastTypes = new SelectList(mastTypes, "Value", "Text"),
                    MachineModels = new SelectList(machineModels, "Value", "Text")
                };
            }

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id.Value);

            if (product == null) return null;

            return new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ProductTypeId = product.ProductTypeId,
                EngineId = product.EngineId,
                MastTypeId = product.MastTypeId,
                MachineModelId = product.MachineModelId,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                LiftingHeight = product.LiftingHeight,
                ClosedHeight = product.ClosedHeight,

                ExistingImageUrls = product.ProductImages.Select(pi => pi.ImageUrl).ToList(),

                Brands = new SelectList(brands, "Value", "Text"),
                Categories = new SelectList(categories, "Value", "Text"),
                ProductTypes = new SelectList(productTypes, "Value", "Text"),
                Engines = new SelectList(engines, "Value", "Text"),
                MastTypes = new SelectList(mastTypes, "Value", "Text"),
                MachineModels = new SelectList(machineModels, "Value", "Text")
            };
        }

        public async Task CreateProductAsync(ProductFormViewModel viewModel)
        {
            var product = new Product
            {
                Name = viewModel.Name,
                BrandId = viewModel.BrandId,
                CategoryId = viewModel.CategoryId,
                ProductTypeId = viewModel.ProductTypeId,
                EngineId = viewModel.EngineId,
                MastTypeId = viewModel.MastTypeId,
                MachineModelId = viewModel.MachineModelId,
                Description = viewModel.Description,
                Price = viewModel.Price,
                Stock = viewModel.Stock,
                LiftingHeight = viewModel.LiftingHeight,
                ClosedHeight = viewModel.ClosedHeight,
                ProductImages = new List<ProductImage>()
            };

            if (viewModel.ImageFiles != null && viewModel.ImageFiles.Length != 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/products");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var file in viewModel.ImageFiles)
                {
                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = $"/images/products/{uniqueFileName}"
                    });
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(ProductFormViewModel viewModel)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (product == null) return false;

            product.Name = viewModel.Name;
            product.BrandId = viewModel.BrandId;
            product.CategoryId = viewModel.CategoryId;
            product.ProductTypeId = viewModel.ProductTypeId;
            product.EngineId = viewModel.EngineId;
            product.MastTypeId = viewModel.MastTypeId;
            product.MachineModelId = viewModel.MachineModelId;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.Stock = viewModel.Stock;
            product.LiftingHeight = viewModel.LiftingHeight;
            product.ClosedHeight = viewModel.ClosedHeight;

            if (viewModel.ImageFiles != null && viewModel.ImageFiles.Length != 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/products");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var file in viewModel.ImageFiles)
                {
                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = $"/images/products/{uniqueFileName}"
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                foreach (var image in product.ProductImages)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CleanupMissingProductImagesAsync()
        {
            var images = await _context.ProductImages.ToListAsync();
            int removedCount = 0;

            foreach (var image in images)
            {
                var filePath = Path.Combine(_environment.WebRootPath, image.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (!File.Exists(filePath))
                {
                    _context.ProductImages.Remove(image);
                    removedCount++;
                }
            }

            if (removedCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            return removedCount;
        }


        public async Task<Product?> GetProductDetailsAsync(int? id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductType)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .Include(p => p.MachineModel)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<ProductListViewModel> GetFilteredProductListAsync(
            string? searchTerm,
            int? categoryId,
            int? brandId,
            int? engineId,
            int? mastTypeId,
            string? sortOrder,
            int page,
            int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.MachineModel)
                .Include(p => p.Category)
                .Include(p => p.Engine)
                .Include(p => p.MastType)
                .Include(p => p.ProductType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p => p.Name.Contains(searchTerm));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            if (brandId.HasValue)
                query = query.Where(p => p.BrandId == brandId);

            if (engineId.HasValue)
                query = query.Where(p => p.EngineId == engineId);

            if (mastTypeId.HasValue)
                query = query.Where(p => p.MastTypeId == mastTypeId);

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name),
            };

            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var brands = await _context.Brands.OrderBy(b => b.Name)
        .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name })
        .ToListAsync();

            var categories = await _context.Categories.OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            var engines = await _context.Engines.OrderBy(e => e.Type)
                .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Type })
                .ToListAsync();

            var mastTypes = await _context.MastTypes.OrderBy(m => m.Name)
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name })
                .ToListAsync();

            return new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                SearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                SelectedBrandId = brandId,
                SelectedEngineId = engineId,
                SelectedMastTypeId = mastTypeId,
                SortOrder = sortOrder,
                Brands = brands,
                Categories = categories,
                Engines = engines,
                MastTypes = mastTypes
            };
        }

    }
}

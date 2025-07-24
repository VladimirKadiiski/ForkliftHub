using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class ProductServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static async Task SeedRequiredEntitiesAsync(ApplicationDbContext context)
        {
            // Add required related entities for foreign keys:
            if (!context.Brands.Any())
            {
                context.Brands.Add(new Brand { Name = "Test Brand" });
            }
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Name = "Test Category" });
            }
            if (!context.ProductTypes.Any())
            {
                context.ProductTypes.Add(new ProductType { Name = "Test ProductType" });
            }
            if (!context.Engines.Any())
            {
                context.Engines.Add(new Engine { Type = "Test Engine" });
            }
            if (!context.MastTypes.Any())
            {
                context.MastTypes.Add(new MastType { Name = "Test MastType" });
            }
            if (!context.MachineModels.Any())
            {
                context.MachineModels.Add(new MachineModel { Name = "Test MachineModel" });
            }
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateProductAsync_AddsProduct()
        {
            var context = GetDbContext();
            await SeedRequiredEntitiesAsync(context);

            var service = new ProductService(context);

            var brand = await context.Brands.FirstAsync();
            var category = await context.Categories.FirstAsync();
            var productType = await context.ProductTypes.FirstAsync();
            var engine = await context.Engines.FirstAsync();
            var mastType = await context.MastTypes.FirstAsync();
            var machineModel = await context.MachineModels.FirstAsync();

            var vm = new ProductFormViewModel
            {
                Name = "Forklift X",
                BrandId = brand.Id,
                CategoryId = category.Id,
                ProductTypeId = productType.Id,
                EngineId = engine.Id,
                MastTypeId = mastType.Id,
                MachineModelId = machineModel.Id,
                Description = "Heavy duty forklift",
                Price = 10000m,
                Stock = 5,
                LiftingHeight = 300,
                ClosedHeight = 200,
                ImageUrl = "http://image.url/forklift.jpg"
            };

            await service.CreateProductAsync(vm);

            var productInDb = await context.Products.FirstOrDefaultAsync(p => p.Name == "Forklift X");
            Assert.NotNull(productInDb);
            Assert.Equal(vm.Description, productInDb.Description);
        }

        [Fact]
        public async Task GetProductByIdAsync_ReturnsCorrectProduct()
        {
            var context = GetDbContext();
            await SeedRequiredEntitiesAsync(context);

            var brand = await context.Brands.FirstAsync();
            var category = await context.Categories.FirstAsync();
            var productType = await context.ProductTypes.FirstAsync();
            var engine = await context.Engines.FirstAsync();
            var mastType = await context.MastTypes.FirstAsync();
            var machineModel = await context.MachineModels.FirstAsync();

            var product = new Product
            {
                Name = "Forklift Y",
                BrandId = brand.Id,
                CategoryId = category.Id,
                ProductTypeId = productType.Id,
                EngineId = engine.Id,
                MastTypeId = mastType.Id,
                MachineModelId = machineModel.Id,
                Description = "Medium duty forklift",
                Price = 8000m,
                Stock = 3,
                LiftingHeight = 250,
                ClosedHeight = 180,
                ImageUrl = "http://image.url/forklifty.jpg"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            var result = await service.GetProductByIdAsync(product.Id);

            Assert.NotNull(result);
            Assert.Equal("Forklift Y", result!.Name);
            Assert.Equal("Medium duty forklift", result.Description);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            var context = GetDbContext();
            await SeedRequiredEntitiesAsync(context);

            var brand = await context.Brands.FirstAsync();
            var category = await context.Categories.FirstAsync();
            var productType = await context.ProductTypes.FirstAsync();
            var engine = await context.Engines.FirstAsync();
            var mastType = await context.MastTypes.FirstAsync();
            var machineModel = await context.MachineModels.FirstAsync();

            var product = new Product
            {
                Name = "Forklift Z",
                BrandId = brand.Id,
                CategoryId = category.Id,
                ProductTypeId = productType.Id,
                EngineId = engine.Id,
                MastTypeId = mastType.Id,
                MachineModelId = machineModel.Id,
                Description = "Old description",
                Price = 7000m,
                Stock = 2,
                LiftingHeight = 220,
                ClosedHeight = 170,
                ImageUrl = "http://image.url/forkliftz.jpg"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            var vm = new ProductFormViewModel
            {
                Id = product.Id,
                Name = "Forklift Z Updated",
                BrandId = brand.Id,
                CategoryId = category.Id,
                ProductTypeId = productType.Id,
                EngineId = engine.Id,
                MastTypeId = mastType.Id,
                MachineModelId = machineModel.Id,
                Description = "Updated description",
                Price = 7500m,
                Stock = 4,
                LiftingHeight = 230,
                ClosedHeight = 175,
                ImageUrl = "http://image.url/forkliftz-updated.jpg"
            };

            var updateResult = await service.UpdateProductAsync(vm);
            Assert.True(updateResult);

            var updatedProduct = await context.Products.FindAsync(product.Id);
            Assert.NotNull(updatedProduct);
            Assert.Equal("Forklift Z Updated", updatedProduct!.Name);
            Assert.Equal("Updated description", updatedProduct.Description);
        }

        [Fact]
        public async Task DeleteProductAsync_RemovesProduct()
        {
            var context = GetDbContext();
            await SeedRequiredEntitiesAsync(context);

            var brand = await context.Brands.FirstAsync();
            var category = await context.Categories.FirstAsync();
            var productType = await context.ProductTypes.FirstAsync();
            var engine = await context.Engines.FirstAsync();
            var mastType = await context.MastTypes.FirstAsync();
            var machineModel = await context.MachineModels.FirstAsync();

            var product = new Product
            {
                Name = "Forklift To Delete",
                BrandId = brand.Id,
                CategoryId = category.Id,
                ProductTypeId = productType.Id,
                EngineId = engine.Id,
                MastTypeId = mastType.Id,
                MachineModelId = machineModel.Id,
                Description = "Will be deleted",
                Price = 6000m,
                Stock = 1,
                LiftingHeight = 210,
                ClosedHeight = 160,
                ImageUrl = "http://image.url/forklift-delete.jpg"
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            await service.DeleteProductAsync(product.Id);

            var deletedProduct = await context.Products.FindAsync(product.Id);
            Assert.Null(deletedProduct);
        }
    }
}

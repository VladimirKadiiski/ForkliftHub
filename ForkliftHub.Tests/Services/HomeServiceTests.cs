using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class HomeServiceTests
    {
        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ForkliftHub_HomeService_{System.Guid.NewGuid()}")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetHomePageProductsAsync_Returns_Empty_When_No_Products()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new HomeService(context);

            // Act
            var result = await service.GetHomePageProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Products);
        }

        [Fact]
        public async Task GetHomePageProductsAsync_Returns_Latest_Products()
        {
            var context = GetInMemoryDbContext();

            // Seed related entities
            var brand = new Brand { Name = "Linde" };
            var model = new MachineModel { Name = "H45D" };
            context.Brands.Add(brand);
            context.MachineModels.Add(model);
            await context.SaveChangesAsync();

            // Add products with valid FK references
            var products = new List<Product>
    {
        new() { Name = "Truck A", Description = "Desc A", BrandId = brand.Id, MachineModelId = model.Id, CategoryId = 1, ProductTypeId = 1, EngineId = 1, MastTypeId = 1, Price = 10000, Stock = 5 },
        new() { Name = "Truck B", Description = "Desc B", BrandId = brand.Id, MachineModelId = model.Id, CategoryId = 1, ProductTypeId = 1, EngineId = 1, MastTypeId = 1, Price = 12000, Stock = 3 },
        new() { Name = "Truck C", Description = "Desc C", BrandId = brand.Id, MachineModelId = model.Id, CategoryId = 1, ProductTypeId = 1, EngineId = 1, MastTypeId = 1, Price = 8000, Stock = 2 }
    };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var service = new HomeService(context);

            var result = await service.GetHomePageProductsAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Products.Count());
            Assert.Equal("Truck C", result.Products.First().Name); // latest by ID
        }


        [Fact]
        public async Task GetHomePageProductsAsync_Returns_Up_To_Specified_Count()
        {
            var context = GetInMemoryDbContext();

            var brand = new Brand { Name = "Toyota" };
            var model = new MachineModel { Name = "8FGCU25" };
            context.Brands.Add(brand);
            context.MachineModels.Add(model);
            await context.SaveChangesAsync();

            for (int i = 1; i <= 10; i++)
            {
                context.Products.Add(new Product
                {
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    BrandId = brand.Id,
                    MachineModelId = model.Id,
                    CategoryId = 1,
                    ProductTypeId = 1,
                    EngineId = 1,
                    MastTypeId = 1,
                    Price = 1000 + i,
                    Stock = i
                });
            }

            await context.SaveChangesAsync();

            var service = new HomeService(context);
            var result = await service.GetHomePageProductsAsync(count: 6);

            Assert.NotNull(result);
            Assert.Equal(6, result.Products.Count());
            Assert.Equal("Product 10", result.Products.First().Name); // newest product first
        }

    }
}

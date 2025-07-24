using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class ProductTypeServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB per test
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsProductType()
        {
            var context = GetDbContext();
            var service = new ProductTypeService(context);
            var viewModel = new ProductTypeViewModel { Name = "Electric" };

            await service.CreateAsync(viewModel);

            Assert.Single(context.ProductTypes);
            Assert.Equal("Electric", context.ProductTypes.First().Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProductTypes()
        {
            var context = GetDbContext();
            context.ProductTypes.AddRange(
                new ProductType { Name = "Diesel" },
                new ProductType { Name = "Hybrid" });
            await context.SaveChangesAsync();

            var service = new ProductTypeService(context);
            var result = (await service.GetAllAsync()).ToList();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectViewModel()
        {
            var context = GetDbContext();
            var pt = new ProductType { Name = "Manual" };
            context.ProductTypes.Add(pt);
            await context.SaveChangesAsync();

            var service = new ProductTypeService(context);
            var result = await service.GetByIdAsync(pt.Id);

            Assert.NotNull(result);
            Assert.Equal("Manual", result!.Name);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_IfExists()
        {
            var context = GetDbContext();
            context.ProductTypes.Add(new ProductType { Name = "LPG" });
            await context.SaveChangesAsync();

            var service = new ProductTypeService(context);
            var exists = await service.ExistsByNameAsync("LPG");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_IfNotExists()
        {
            var context = GetDbContext();
            var service = new ProductTypeService(context);

            var exists = await service.ExistsByNameAsync("NonExisting");

            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesName()
        {
            var context = GetDbContext();
            var pt = new ProductType { Name = "OldName" };
            context.ProductTypes.Add(pt);
            await context.SaveChangesAsync();

            var service = new ProductTypeService(context);
            var updatedVm = new ProductTypeViewModel { Id = pt.Id, Name = "NewName" };

            await service.UpdateAsync(updatedVm);

            var updated = await context.ProductTypes.FindAsync(pt.Id);
            Assert.Equal("NewName", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesProductType()
        {
            var context = GetDbContext();
            var pt = new ProductType { Name = "Temporary" };
            context.ProductTypes.Add(pt);
            await context.SaveChangesAsync();

            var service = new ProductTypeService(context);
            await service.DeleteAsync(pt.Id);

            Assert.Empty(context.ProductTypes);
        }
    }
}

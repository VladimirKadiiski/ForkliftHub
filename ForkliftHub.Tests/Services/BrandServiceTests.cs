using FluentAssertions;
using ForkliftHub.Services;
using ForkliftHub.Data;
using ForkliftHub.Models;
using Microsoft.EntityFrameworkCore;
using ForkliftHub.ViewModels;

namespace ForkliftHub.Tests.Services
{
    public class BrandServiceTests
    {
        private static async Task<ApplicationDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"ForkliftHub_Test_{System.Guid.NewGuid()}")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Brands.AddRange(
                new Brand { Id = 1, Name = "Toyota" },
                new Brand { Id = 2, Name = "Caterpillar" }
            );
            await context.SaveChangesAsync();
            return context;
        }
        private static readonly string[] expected = new[] { "Toyota", "Caterpillar" };

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBrands()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Select(b => b.Name).Should().Contain(expected);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBrand()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);

            var result = await service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Toyota");
        }

        [Fact]
        public async Task CreateAsync_ShouldAddBrand()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);

            await service.CreateAsync(new BrandViewModel { Name = "Linde" });

            var brands = await context.Brands.ToListAsync();
            brands.Should().HaveCount(3);
            brands.Select(b => b.Name).Should().Contain("Linde");
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingBrand()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);

            var brand = await service.GetByIdAsync(1);
            brand!.Name = "Toyota Updated";

            await service.UpdateAsync(brand);

            var updated = await context.Brands.FindAsync(1);
            updated!.Name.Should().Be("Toyota Updated");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBrand()
        {
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);

            await service.DeleteAsync(1);

            var brand = await context.Brands.FindAsync(1);
            brand.Should().BeNull();
        }

        [Fact]
        public async Task BrandExistsAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);
            context.Brands.Add(new Brand { Name = "Exists" });
            await context.SaveChangesAsync();
                        
            // Act
            var result = await service.BrandExistsAsync("Exists");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task BrandExistsAsync_ShouldExcludeId()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var service = new BrandService(context);
            var brand = new Brand { Name = "Unique" };
            context.Brands.Add(brand);
            await context.SaveChangesAsync();
                        
            // Act
            var result = await service.BrandExistsAsync("Unique", excludeId: brand.Id);

            // Assert
            Assert.False(result);
        }
    }
}

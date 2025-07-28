using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class CategoryServiceTests
    {
        private static ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"CategoryDb_{System.Guid.NewGuid()}")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldAddCategory()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new CategoryService(context);
            var vm = new CategoryViewModel { Name = "Electric" };

            // Act
            await service.CreateCategoryAsync(vm);

            // Assert
            var category = await context.Categories.FirstOrDefaultAsync();
            Assert.NotNull(category);
            Assert.Equal("Electric", category.Name);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAll()
        {
            // Arrange
            using var context = GetDbContext();
            context.Categories.AddRange(
                new Category { Name = "Electric" },
                new Category { Name = "Diesel" });
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Electric");
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnCorrectItem()
        {
            // Arrange
            using var context = GetDbContext();
            var category = new Category { Name = "Hybrid" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.GetCategoryByIdAsync(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hybrid", result!.Name);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldUpdateCategory()
        {
            // Arrange
            using var context = GetDbContext();
            var category = new Category { Name = "OldName" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);
            var updatedVm = new CategoryViewModel { Id = category.Id, Name = "NewName" };

            // Act
            await service.UpdateCategoryAsync(updatedVm);

            // Assert
            var updated = await context.Categories.FindAsync(category.Id);
            Assert.Equal("NewName", updated!.Name);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldRemoveCategory()
        {
            // Arrange
            using var context = GetDbContext();
            var category = new Category { Name = "ToDelete" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            await service.DeleteCategoryAsync(category.Id);

            // Assert
            var deleted = await context.Categories.FindAsync(category.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task CategoryExistsAsync_ShouldReturnTrueIfExists()
        {
            // Arrange
            using var context = GetDbContext();
            context.Categories.Add(new Category { Name = "Exists" });
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.CategoryExistsAsync("Exists");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CategoryExistsAsync_ShouldExcludeId()
        {
            // Arrange
            using var context = GetDbContext();
            var category = new Category { Name = "Unique" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var service = new CategoryService(context);

            // Act
            var result = await service.CategoryExistsAsync("Unique", excludeId: category.Id);

            // Assert
            Assert.False(result);
        }
    }
}

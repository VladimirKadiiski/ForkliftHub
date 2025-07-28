using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using ForkliftHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class AdminDashboardServiceTests
    {
        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetDashboardDataAsync_Returns_Correct_Counts()
        {
            // Arrange
            using var context = GetInMemoryDbContext();

            // Seed Products
            context.Products.AddRange(
                new Product { Name = "P1", Description = "D1" },
                new Product { Name = "P2", Description = "D2" });

            // Seed Users
            context.Users.AddRange(
                new ApplicationUser { UserName = "user1@example.com", Email = "user1@example.com" },
                new ApplicationUser { UserName = "user2@example.com", Email = "user2@example.com" });

            // Seed Reviews
            context.Reviews.AddRange(
                new Review { Rating = 5, Comment = "Great!", UserId = "U1" },
                new Review { Rating = 4, Comment = "Good!", UserId = "U2" },
                new Review { Rating = 3, Comment = "Okay!", UserId = "U3" });

            await context.SaveChangesAsync();

            var service = new AdminDashboardService(context);

            // Act
            AdminDashboardViewModel result = await service.GetDashboardDataAsync();

            // Assert
            Assert.Equal(2, result.ProductCount);
            Assert.Equal(2, result.UserCount);
            Assert.Equal(3, result.ReviewCount);
        }

        [Fact]
        public async Task GetDashboardDataAsync_Returns_Zeros_When_Empty()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var service = new AdminDashboardService(context);

            // Act
            AdminDashboardViewModel result = await service.GetDashboardDataAsync();

            // Assert
            Assert.Equal(0, result.ProductCount);
            Assert.Equal(0, result.UserCount);
            Assert.Equal(0, result.ReviewCount);
        }
    }
}

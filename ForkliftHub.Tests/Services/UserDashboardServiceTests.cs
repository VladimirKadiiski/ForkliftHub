using ForkliftHub.Data;
using ForkliftHub.Models;
using ForkliftHub.Services;
using Microsoft.EntityFrameworkCore;

namespace ForkliftHub.Tests.Services
{
    public class UserDashboardServiceTests
    {
        private static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetDashboardDataAsync_ReturnsOnlyCurrentUsersReviews()
        {
            // Arrange
            var userId = "user-123";
            using var context = GetInMemoryDbContext();

            var product = new Product
            {
                Name = "Linde",
                Description = "Some description"
            };

            context.Products.Add(product);

            context.Reviews.AddRange(
                new Review { Rating = 5, Comment = "Great", UserId = userId, Product = product },
                new Review { Rating = 4, Comment = "Good", UserId = userId, Product = product },
                new Review { Rating = 3, Comment = "Not yours", UserId = "someone-else", Product = product }
            );

            await context.SaveChangesAsync();
            var service = new UserDashboardService(context);

            // Act
            var result = await service.GetDashboardDataAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.MyReviews.Count);
            Assert.All(result.MyReviews, r => Assert.Equal(userId, r.UserId));
        }

        [Fact]
        public async Task GetDashboardDataAsync_ReturnsEmptyList_IfUserHasNoReviews()
        {
            // Arrange
            var userId = "user-no-reviews";
            using var context = GetInMemoryDbContext();
            var service = new UserDashboardService(context);

            // Act
            var result = await service.GetDashboardDataAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.MyReviews);
        }
    }
}

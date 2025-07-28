using ForkliftHub.Models;

namespace ForkliftHub.ViewModels
{
    public class UserDashboardViewModel
    {
        public List<Review> MyReviews { get; set; } = new();
        public string? UserName { get; set; }

        // Extend this with:
        // public List<Product> RecentlyViewed { get; set; }
        // public List<Product> FavoriteProducts { get; set; }
    }
}

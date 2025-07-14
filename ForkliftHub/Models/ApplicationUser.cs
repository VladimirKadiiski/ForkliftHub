using Microsoft.AspNetCore.Identity;

namespace ForkliftHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}

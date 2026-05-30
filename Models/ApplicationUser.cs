using Microsoft.AspNetCore.Identity;

namespace WebBanHang2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? AvatarUrl { get; set; }
        public string? FullName { get; set; }
    }
}

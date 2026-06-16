using Microsoft.AspNetCore.Identity;

namespace PhamDangKhoa_W345_C2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? AvatarUrl { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
    }
}

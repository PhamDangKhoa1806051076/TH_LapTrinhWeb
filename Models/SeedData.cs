using Microsoft.AspNetCore.Identity;

namespace PhamDangKhoa_W345_C2.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed tất cả roles theo SD.cs
            string[] roleNames = { SD.Role_Admin, SD.Role_Customer, SD.Role_Employee, SD.Role_Company };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminEmail = "admin@sgnstore.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "123456A");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SD.Role_Admin);
                }
            }
            else
            {
                // Cập nhật mật khẩu admin
                var token = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                await userManager.ResetPasswordAsync(adminUser, token, "123456A");

                // Cập nhật avatar admin
                adminUser.AvatarUrl = "/images/avataradmin.jpg";
                await userManager.UpdateAsync(adminUser);

                // Đảm bảo admin có role Admin
                if (!await userManager.IsInRoleAsync(adminUser, SD.Role_Admin))
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }
        }
    }
}

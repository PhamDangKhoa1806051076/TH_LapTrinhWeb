using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebBanHang2.Models;

namespace WebBanHang2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl ?? "/");
                }
                ModelState.AddModelError(string.Empty, "Đăng nhập không thành công.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(IFormFile avatarFile, string? fullName)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(fullName) && user.FullName != fullName)
            {
                user.FullName = fullName;
                isUpdated = true;
            }

            if (avatarFile != null)
            {
                var dir = Path.Combine("wwwroot", "users", "avatars");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var savePath = Path.Combine(dir, fileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }
                user.AvatarUrl = "/users/avatars/" + fileName;
                isUpdated = true;
            }

            if (isUpdated)
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";
            }

            return RedirectToAction("Profile");
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

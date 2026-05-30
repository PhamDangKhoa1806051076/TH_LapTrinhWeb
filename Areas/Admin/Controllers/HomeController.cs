using Microsoft.AspNetCore.Mvc;

namespace PhamDangKhoa_W345_C2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

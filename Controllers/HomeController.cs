using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBanHang2.Models;
using WebBanHang2.Repositories;
using System.Linq;

namespace WebBanHang2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    // Action xử lý request trả về trang chủ (Index)
    public IActionResult Index()
    {
        // Lấy 8 sản phẩm nổi bật cho trang chủ
        var products = _productRepository.GetAll().Take(8).ToList();
        return View(products);
    }

    // Action xử lý request trả về trang Chính sách bảo mật (Privacy)
    public IActionResult Privacy()
    {
        return View();
    }

    // Action xử lý và hiển thị trang báo lỗi khi ứng dụng gặp sự cố
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhamDangKhoa_W345_C2.Models;
using PhamDangKhoa_W345_C2.Repositories;

namespace PhamDangKhoa_W345_C2.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Trang Cửa hàng dành cho khách hàng
        public IActionResult List(int? categoryId, string? searchQuery, int page = 1)
        {
            int pageSize = 6;
            var allProducts = _productRepository.GetAll();

            // Lọc theo danh mục
            if (categoryId.HasValue)
            {
                allProducts = allProducts.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Lọc theo từ khóa tìm kiếm (Tên sản phẩm)
            if (!string.IsNullOrEmpty(searchQuery))
            {
                allProducts = allProducts.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int totalProducts = allProducts.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Categories = _categoryRepository.GetAllCategories();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.SearchQuery = searchQuery;

            return View(products);
        }

        // Hiển thị thông tin chi tiết của một sản phẩm
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(); 
            }
            
            // Lấy các sản phẩm cùng danh mục (loại trừ sản phẩm hiện tại)
            var relatedProducts = _productRepository.GetAll()
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(4)
                .ToList();
                
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
    }
}

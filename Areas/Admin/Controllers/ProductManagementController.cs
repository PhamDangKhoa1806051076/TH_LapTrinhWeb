using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanHang2.Models;
using WebBanHang2.Repositories;

namespace WebBanHang2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductManagementController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        
        public ProductManagementController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public IActionResult Index(int page = 1)
        {
            int pageSize = 6;
            var allProducts = _productRepository.GetAll();
            int totalProducts = allProducts.Count();
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

            var products = allProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalProducts;

            return View(products);
        }

        // Hiển thị form thêm mới sản phẩm
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // Xử lý dữ liệu khi người dùng submit form thêm mới
        [HttpPost]
        public async Task<IActionResult> Add(Product product, List<IFormFile> newFiles, string? mainImageIdentifier)
        {
            if (ModelState.IsValid)
            {
                var uploadedUrls = new List<string>();
                
                // Save all new files
                if (newFiles != null && newFiles.Count > 0)
                {
                    foreach (var file in newFiles)
                    {
                        uploadedUrls.Add(await SaveImage(file));
                    }
                }

                // Determine main image
                if (!string.IsNullOrEmpty(mainImageIdentifier) && mainImageIdentifier.StartsWith("new_"))
                {
                    if (int.TryParse(mainImageIdentifier.Split('_')[1], out int idx) && idx < uploadedUrls.Count)
                    {
                        product.ImageUrl = uploadedUrls[idx];
                        uploadedUrls.RemoveAt(idx);
                    }
                }
                else if (uploadedUrls.Count > 0)
                {
                    // Fallback: take first as main
                    product.ImageUrl = uploadedUrls[0];
                    uploadedUrls.RemoveAt(0);
                }

                // Remaining goes to sub images (max 3)
                product.ImageUrls = uploadedUrls.Take(3).ToList();

                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var dir = Path.Combine("wwwroot", "products", "Images");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(dir, fileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/products/Images/" + fileName;
        }

        // Hiển thị form cập nhật thông tin sản phẩm
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(); 
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Xử lý dữ liệu khi người dùng submit form cập nhật
        [HttpPost]
        public async Task<IActionResult> Update(Product product, List<IFormFile> newFiles, string? mainImageIdentifier, string? deletedImages)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _productRepository.GetById(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Quantity = product.Quantity;

                // Handle deletions
                var allCurrentImages = new List<string>();
                if (!string.IsNullOrEmpty(existingProduct.ImageUrl)) allCurrentImages.Add(existingProduct.ImageUrl);
                if (existingProduct.ImageUrls != null) allCurrentImages.AddRange(existingProduct.ImageUrls);

                if (!string.IsNullOrEmpty(deletedImages))
                {
                    var deletedList = deletedImages.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    allCurrentImages.RemoveAll(img => deletedList.Contains(img));
                }

                // Handle new uploads
                var newUploadedUrls = new List<string>();
                if (newFiles != null && newFiles.Count > 0)
                {
                    foreach (var file in newFiles)
                    {
                        newUploadedUrls.Add(await SaveImage(file));
                    }
                }

                // Re-determine main image
                existingProduct.ImageUrl = null;
                
                if (!string.IsNullOrEmpty(mainImageIdentifier))
                {
                    if (mainImageIdentifier.StartsWith("new_"))
                    {
                        if (int.TryParse(mainImageIdentifier.Split('_')[1], out int idx) && idx < newUploadedUrls.Count)
                        {
                            existingProduct.ImageUrl = newUploadedUrls[idx];
                            newUploadedUrls.RemoveAt(idx);
                        }
                    }
                    else if (allCurrentImages.Contains(mainImageIdentifier))
                    {
                        existingProduct.ImageUrl = mainImageIdentifier;
                        allCurrentImages.Remove(mainImageIdentifier);
                    }
                }

                // Combine remaining current and new images for sub-images
                var remainingImages = new List<string>();
                remainingImages.AddRange(allCurrentImages);
                remainingImages.AddRange(newUploadedUrls);
                
                // If main image still not set, pick the first available
                if (string.IsNullOrEmpty(existingProduct.ImageUrl) && remainingImages.Count > 0)
                {
                    existingProduct.ImageUrl = remainingImages[0];
                    remainingImages.RemoveAt(0);
                }

                existingProduct.ImageUrls = remainingImages.Take(3).ToList();

                _productRepository.Update(existingProduct);
                return RedirectToAction("Index");
            }
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // Hiển thị trang xác nhận xóa sản phẩm
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound(); 
            }
            return View(product);
        }

        // Xử lý thao tác xóa khi người dùng xác nhận
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStock(int id, int quantity)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                if (quantity < 0) quantity = 0;
                product.Quantity = quantity;
                _productRepository.Update(product);
            }
            return RedirectToAction("Index");
        }
    }
}

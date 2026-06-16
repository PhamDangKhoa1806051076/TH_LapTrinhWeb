using Bai2.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bai2.Repositories
{
    /// <summary>
    /// MockProductRepository - Lớp triển khai IProductRepository dùng dữ liệu giả lập trong bộ nhớ (RAM).
    /// Theo giáo trình trang 26-27: dùng "Mock Data" thay thế database thật trong giai đoạn học tập.
    /// Vòng đời: Singleton (đăng ký trong Program.cs) → dữ liệu tồn tại suốt phiên chạy ứng dụng.
    /// </summary>
    public class MockProductRepository : IProductRepository
    {
        // Danh sách sản phẩm lưu trong bộ nhớ (thay thế database)
        private readonly List<Product> _products;

        /// <summary>
        /// Constructor: khởi tạo dữ liệu mẫu ban đầu (2 sản phẩm).
        /// Theo giáo trình trang 26: dữ liệu mock dùng để test CRUD mà không cần database thật.
        /// </summary>
        public MockProductRepository()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop MacBook Neo",        Price = 1080, Description = "13 inch A18 Pro 8GB/256GB", CategoryId = 1,
                    ImageUrl = "/images/macbook-neo.jpg",
                    ImageUrls = new List<string> { "/images/macbook-neo-goc3.jpg" } },
                new Product { Id = 2, Name = "iPhone 17 Pro Max",         Price = 1200, Description = "iOS 26, Chip Apple A19 Pro, 6 nhân, 4.25 GHz", CategoryId = 2,
                    ImageUrl = "/images/iPhone 17 Pro Max.jpeg" },
                new Product { Id = 3, Name = "Dell XPS 15 9530",          Price = 1899, Description = "15.6 inch OLED, Intel Core i9-13900H, 32GB RAM, 1TB SSD, RTX 4070", CategoryId = 1,
                    ImageUrl = "/images/dell-xps-15-9530-i7-71015716-3-750x500.jpg" },
                new Product { Id = 4, Name = "Samsung Galaxy S25 Ultra",  Price = 1299, Description = "Android 15, Snapdragon 8 Elite, 12GB RAM, 256GB, Camera 200MP", CategoryId = 2,
                    ImageUrl = "/images/Samsung Galaxy S25 Ultra.jpg" },
                new Product { Id = 5, Name = "ASUS ROG Zephyrus G14",     Price = 1649, Description = "14 inch QHD 165Hz, AMD Ryzen 9 8945HS, 32GB RAM, 1TB SSD, RX 7900S", CategoryId = 1,
                    ImageUrl = "/images/asus-gaming-rog-zephyrus-g14-ga403wm-r9-ai-hx370-qs051ws-1-639016549705989353-750x500.jpg" },
                new Product { Id = 6, Name = "Google Pixel 9 Pro",        Price = 999,  Description = "Android 15, Google Tensor G4, 16GB RAM, 256GB, Camera 50MP Zeiss", CategoryId = 2,
                    ImageUrl = "/images/Google Pixel 9 Pro.jpg" },
                new Product { Id = 7, Name = "MacBook Pro 14 M4 Pro",     Price = 1999, Description = "14 inch Liquid Retina XDR, Apple M4 Pro 12-core, 24GB RAM, 512GB SSD", CategoryId = 1,
                    ImageUrl = "/images/macbook-pro-14-inch-m4-pro-24gb-1tb-20gpu-bac-1-639104240462766154-750x500.jpg" },
                new Product { Id = 8, Name = "OnePlus 13",                Price = 899,  Description = "Android 15, Snapdragon 8 Elite, 16GB RAM, 512GB, Hasselblad Camera 50MP", CategoryId = 2,
                    ImageUrl = "/images/OnePlus 13.jpg" },
            };
        }

        /// <summary>
        /// Lấy toàn bộ danh sách sản phẩm.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        /// <summary>
        /// Tìm sản phẩm theo Id. Trả về null nếu không tìm thấy.
        /// </summary>
        public Product? GetById(int id)
        {
            // FirstOrDefault: trả null thay vì ném exception nếu không tìm thấy
            return _products.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Thêm sản phẩm mới vào danh sách.
        ///
        /// ⚠️ LỖI GỐC (giáo trình trang 26, dòng gốc):
        ///     product.Id = _products.Max(p => p.Id) + 1;
        ///
        /// VẤN ĐỀ: Khi danh sách _products rỗng (sau khi xóa hết),
        ///     _products.Max() ném ra InvalidOperationException:
        ///     "Sequence contains no elements"
        ///     → Chương trình crash khi thêm sản phẩm đầu tiên sau khi xóa hết.
        ///
        /// ✅ SỬA LỖI: Kiểm tra danh sách có rỗng không trước khi gọi Max():
        ///     - Nếu rỗng (Any() = false) → gán Id = 1 (bắt đầu lại từ đầu)
        ///     - Nếu có phần tử → lấy Id lớn nhất + 1 như bình thường
        /// </summary>
        public void Add(Product product)
        {
            // _products.Any() = true nếu còn ít nhất 1 phần tử, false nếu rỗng.
            // Dùng toán tử 3 ngôi (?:) để tránh gọi Max() khi list rỗng.
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            _products.Add(product);
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm.
        /// Tìm vị trí (index) của sản phẩm trong list, rồi thay thế toàn bộ object.
        /// </summary>
        public void Update(Product product)
        {
            // FindIndex trả về -1 nếu không tìm thấy → kiểm tra trước khi cập nhật
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }

        /// <summary>
        /// Xóa sản phẩm khỏi danh sách theo Id.
        /// </summary>
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            // Sau khi xóa hết: _products.Count = 0
            // Lần Add() tiếp theo: Any() = false → Id = 1, không bị crash.
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using PhamDangKhoa_W345_C2.Models;

namespace PhamDangKhoa_W345_C2.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        
        public MockProductRepository()
        {
            // Khởi tạo danh sách sản phẩm mẫu (mock data)
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop HP Pavilion", Price = 15000000, Description = "Laptop HP Pavilion mỏng nhẹ, hiệu năng cao phù hợp cho sinh viên và dân văn phòng.", CategoryId = 1, ImageUrl = "/images/Hp1.png", ImageUrls = new List<string> { "/images/Hp2.png" } },
                new Product { Id = 2, Name = "Laptop Lenovo ThinkPad", Price = 25000000, Description = "Laptop doanh nhân Lenovo ThinkPad siêu bền bỉ, bàn phím gõ cực êm.", CategoryId = 1, ImageUrl = "/images/Lap1.png", ImageUrls = new List<string> { "/images/Lap12.png" } },
                new Product { Id = 3, Name = "Laptop Acer Nitro 5", Price = 22000000, Description = "Laptop gaming quốc dân Acer Nitro 5 với tản nhiệt cực tốt và cấu hình mạnh mẽ.", CategoryId = 1, ImageUrl = "/images/Nitro1.png", ImageUrls = new List<string> { "/images/Nitro2.png" } },
                new Product { Id = 4, Name = "Laptop HP OMEN", Price = 35000000, Description = "Laptop gaming cao cấp HP OMEN với thiết kế sang trọng, hiệu năng đỉnh cao.", CategoryId = 1, ImageUrl = "/images/OM1.png", ImageUrls = new List<string> { "/images/OM2.png" } },
                new Product { Id = 5, Name = "Laptop Asus ROG", Price = 30000000, Description = "Laptop chuyên game Asus ROG mang lại trải nghiệm chơi game tuyệt vời.", CategoryId = 1, ImageUrl = "/images/80.jpg", ImageUrls = new List<string> { "/images/81.jpg" } },
                new Product { Id = 6, Name = "Laptop Dell XPS 15", Price = 40000000, Description = "Laptop Dell XPS 15 với màn hình vô cực tuyệt đẹp, dành cho người dùng sáng tạo.", CategoryId = 1, ImageUrl = "/images/OIP.jpg", ImageUrls = new List<string> { "/images/OIP (1).jpg" } },
                new Product { Id = 7, Name = "PC Gaming Alpha", Price = 28000000, Description = "Bộ PC Gaming Alpha với tản nhiệt nước, vỏ case trong suốt tuyệt đẹp.", CategoryId = 2, ImageUrl = "/images/PCA.jpg", ImageUrls = new List<string> { "/images/PCA2.png" } },
                new Product { Id = 8, Name = "PC Đồ Họa Beta", Price = 32000000, Description = "PC chuyên dụng cho dân đồ họa 3D, dựng phim với bộ vi xử lý đa nhân siêu tốc.", CategoryId = 2, ImageUrl = "/images/PCB.jpg", ImageUrls = new List<string> { "/images/PCB2.png" } },
                new Product { Id = 9, Name = "PC Văn Phòng Core i5", Price = 10000000, Description = "Máy tính để bàn cấu hình Core i5 nhỏ gọn, hoạt động êm ái thích hợp cho không gian làm việc.", CategoryId = 2, ImageUrl = "/images/PCA2.png" },
                new Product { Id = 10, Name = "PC Workstation Xeon", Price = 45000000, Description = "Máy trạm Workstation Xeon sức mạnh vượt trội cho mọi tác vụ nặng nhất.", CategoryId = 2, ImageUrl = "/images/PCB2.png" },
                new Product { Id = 11, Name = "Chuột Gaming Logitech", Price = 1200000, Description = "Chuột chơi game không dây siêu nhẹ.", CategoryId = 3, ImageUrl = "https://picsum.photos/300/300?random=11" },
                new Product { Id = 12, Name = "Bàn phím cơ Razer", Price = 2500000, Description = "Bàn phím cơ cao cấp với đèn LED RGB.", CategoryId = 3, ImageUrl = "https://picsum.photos/300/300?random=12" },
                new Product { Id = 13, Name = "Tai nghe Sony", Price = 3000000, Description = "Tai nghe chống ồn chủ động.", CategoryId = 3, ImageUrl = "https://picsum.photos/300/300?random=13" },
                new Product { Id = 14, Name = "Màn hình Dell UltraSharp", Price = 8000000, Description = "Màn hình 27 inch 4K IPS.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=14" },
                new Product { Id = 15, Name = "Ổ cứng SSD Samsung 1TB", Price = 2000000, Description = "Ổ cứng SSD NVMe tốc độ cao.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=15" },
                new Product { Id = 16, Name = "RAM Corsair 16GB", Price = 1500000, Description = "RAM DDR4 3200MHz.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=16" },
                new Product { Id = 17, Name = "Card đồ họa RTX 3060", Price = 9000000, Description = "Card đồ họa chơi game tầm trung.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=17" },
                new Product { Id = 18, Name = "Mainboard ASUS ROG", Price = 4000000, Description = "Bo mạch chủ cao cấp cho Intel.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=18" },
                new Product { Id = 19, Name = "Tản nhiệt nước NZXT", Price = 3500000, Description = "Tản nhiệt AIO có màn hình LCD.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=19" },
                new Product { Id = 20, Name = "Nguồn Corsair 750W", Price = 2200000, Description = "Nguồn máy tính chuẩn 80 Plus Gold.", CategoryId = 4, ImageUrl = "https://picsum.photos/300/300?random=20" }
            };
        }
        
        public IEnumerable<Product> GetAll()
        {
            // Trả về toàn bộ danh sách sản phẩm
            return _products;
        }
        
        public Product GetById(int id)
        {
            // Tìm và trả về sản phẩm đầu tiên có ID khớp với tham số truyền vào
            return _products.FirstOrDefault(p => p.Id == id);
        }
        
        public void Add(Product product)
        {
            // Tự động sinh ID cho sản phẩm mới
            product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            // Thêm sản phẩm vào danh sách
            _products.Add(product);
        }
        
        public void Update(Product product)
        {
            // Tìm vị trí của sản phẩm trong danh sách
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                // Cập nhật thông tin sản phẩm nếu tìm thấy
                _products[index] = product;
            }
        }
        
        public void Delete(int id)
        {
            // Tìm sản phẩm theo ID
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                // Xóa sản phẩm khỏi danh sách nếu tồn tại
                _products.Remove(product);
            }
        }
    }
}

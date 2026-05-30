using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebBanHang2.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "Desktop" },
                new Category { Id = 3, Name = "Gaming Gear" },
                new Category { Id = 4, Name = "Linh kiện" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop HP Pavilion", Price = 15000000, Description = "Laptop HP Pavilion mỏng nhẹ, hiệu năng cao phù hợp cho sinh viên và dân văn phòng.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/Hp1.png" },
                new Product { Id = 2, Name = "Laptop Lenovo ThinkPad", Price = 25000000, Description = "Laptop doanh nhân Lenovo ThinkPad siêu bền bỉ, bàn phím gõ cực êm.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/Lap1.png" },
                new Product { Id = 3, Name = "Laptop Acer Nitro 5", Price = 22000000, Description = "Laptop gaming quốc dân Acer Nitro 5 với tản nhiệt cực tốt và cấu hình mạnh mẽ.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/Nitro1.png" },
                new Product { Id = 4, Name = "Laptop HP OMEN", Price = 35000000, Description = "Laptop gaming cao cấp HP OMEN với thiết kế sang trọng, hiệu năng đỉnh cao.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/OM1.png" },
                new Product { Id = 5, Name = "Laptop Asus ROG", Price = 30000000, Description = "Laptop chuyên game Asus ROG mang lại trải nghiệm chơi game tuyệt vời.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/80.jpg" },
                new Product { Id = 6, Name = "Laptop Dell XPS 15", Price = 40000000, Description = "Laptop Dell XPS 15 với màn hình vô cực tuyệt đẹp, dành cho người dùng sáng tạo.", CategoryId = 1, Quantity = 50, ImageUrl = "/images/OIP.jpg" },
                new Product { Id = 7, Name = "PC Gaming Alpha", Price = 28000000, Description = "Bộ PC Gaming Alpha với tản nhiệt nước, vỏ case trong suốt tuyệt đẹp.", CategoryId = 2, Quantity = 50, ImageUrl = "/images/PCA.jpg" },
                new Product { Id = 8, Name = "PC Đồ Họa Beta", Price = 32000000, Description = "PC chuyên dụng cho dân đồ họa 3D, dựng phim với bộ vi xử lý đa nhân siêu tốc.", CategoryId = 2, Quantity = 50, ImageUrl = "/images/PCB.jpg" },
                new Product { Id = 9, Name = "PC Văn Phòng Core i5", Price = 10000000, Description = "Máy tính để bàn cấu hình Core i5 nhỏ gọn, hoạt động êm ái thích hợp cho không gian làm việc.", CategoryId = 2, Quantity = 50, ImageUrl = "/images/PCA2.png" },
                new Product { Id = 10, Name = "PC Workstation Xeon", Price = 45000000, Description = "Máy trạm Workstation Xeon sức mạnh vượt trội cho mọi tác vụ nặng nhất.", CategoryId = 2, Quantity = 50, ImageUrl = "/images/PCB2.png" },
                new Product { Id = 11, Name = "Chuột Gaming Logitech", Price = 1200000, Description = "Chuột chơi game không dây siêu nhẹ.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=11" },
                new Product { Id = 12, Name = "Bàn phím cơ Razer", Price = 2500000, Description = "Bàn phím cơ cao cấp với đèn LED RGB.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=12" },
                new Product { Id = 13, Name = "Tai nghe Sony", Price = 3000000, Description = "Tai nghe chống ồn chủ động.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=13" },
                new Product { Id = 14, Name = "Màn hình Dell UltraSharp", Price = 8000000, Description = "Màn hình 27 inch 4K IPS.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=14" },
                new Product { Id = 15, Name = "Ổ cứng SSD Samsung 1TB", Price = 2000000, Description = "Ổ cứng SSD NVMe tốc độ cao.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=15" },
                new Product { Id = 16, Name = "RAM Corsair 16GB", Price = 1500000, Description = "RAM DDR4 3200MHz.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=16" },
                new Product { Id = 17, Name = "Card đồ họa RTX 3060", Price = 9000000, Description = "Card đồ họa chơi game tầm trung.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=17" },
                new Product { Id = 18, Name = "Mainboard ASUS ROG", Price = 4000000, Description = "Bo mạch chủ cao cấp cho Intel.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=18" },
                new Product { Id = 19, Name = "Tản nhiệt nước NZXT", Price = 3500000, Description = "Tản nhiệt AIO có màn hình LCD.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=19" },
                new Product { Id = 20, Name = "Nguồn Corsair 750W", Price = 2200000, Description = "Nguồn máy tính chuẩn 80 Plus Gold.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=20" },
                new Product { Id = 21, Name = "MacBook Pro M2", Price = 35000000, Description = "Laptop Apple với chip M2 siêu mạnh mẽ.", CategoryId = 1, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=21" },
                new Product { Id = 22, Name = "Laptop LG Gram 16", Price = 38000000, Description = "Laptop siêu nhẹ chỉ 1.1kg, pin trâu.", CategoryId = 1, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=22" },
                new Product { Id = 23, Name = "PC Mini Intel NUC", Price = 12000000, Description = "Máy tính siêu nhỏ gọn phù hợp tivi phòng khách.", CategoryId = 2, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=23" },
                new Product { Id = 24, Name = "Mac Studio M2 Max", Price = 55000000, Description = "Máy trạm Apple hiệu năng đồ họa cực đỉnh.", CategoryId = 2, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=24" },
                new Product { Id = 25, Name = "Tay cầm Xbox Series X", Price = 1500000, Description = "Tay cầm chơi game tốt nhất trên Windows.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=25" },
                new Product { Id = 26, Name = "Webcam Logitech C920", Price = 1800000, Description = "Webcam chuẩn Full HD cho streamer.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=26" },
                new Product { Id = 27, Name = "Loa Bluetooth JBL", Price = 2000000, Description = "Loa di động âm thanh sống động.", CategoryId = 3, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=27" },
                new Product { Id = 28, Name = "Ổ cứng HDD Seagate 2TB", Price = 1400000, Description = "Ổ cứng lưu trữ dữ liệu dung lượng cao.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=28" },
                new Product { Id = 29, Name = "Vỏ Case NZXT H510", Price = 1900000, Description = "Vỏ máy tính bằng thép với mặt kính cường lực.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=29" },
                new Product { Id = 30, Name = "Quạt tản nhiệt Noctua", Price = 800000, Description = "Quạt tản nhiệt siêu êm, lưu lượng gió lớn.", CategoryId = 4, Quantity = 50, ImageUrl = "https://picsum.photos/300/300?random=30" }
            );
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebBanHang2.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Laptop" },
                    { 2, "Desktop" },
                    { 3, "Gaming Gear" },
                    { 4, "Linh kiện" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "ImageUrls", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Laptop HP Pavilion mỏng nhẹ, hiệu năng cao phù hợp cho sinh viên và dân văn phòng.", "/images/Hp1.png", null, "Laptop HP Pavilion", 15000000m },
                    { 2, 1, "Laptop doanh nhân Lenovo ThinkPad siêu bền bỉ, bàn phím gõ cực êm.", "/images/Lap1.png", null, "Laptop Lenovo ThinkPad", 25000000m },
                    { 3, 1, "Laptop gaming quốc dân Acer Nitro 5 với tản nhiệt cực tốt và cấu hình mạnh mẽ.", "/images/Nitro1.png", null, "Laptop Acer Nitro 5", 22000000m },
                    { 4, 1, "Laptop gaming cao cấp HP OMEN với thiết kế sang trọng, hiệu năng đỉnh cao.", "/images/OM1.png", null, "Laptop HP OMEN", 35000000m },
                    { 5, 1, "Laptop chuyên game Asus ROG mang lại trải nghiệm chơi game tuyệt vời.", "/images/80.jpg", null, "Laptop Asus ROG", 30000000m },
                    { 6, 1, "Laptop Dell XPS 15 với màn hình vô cực tuyệt đẹp, dành cho người dùng sáng tạo.", "/images/OIP.jpg", null, "Laptop Dell XPS 15", 40000000m },
                    { 7, 2, "Bộ PC Gaming Alpha với tản nhiệt nước, vỏ case trong suốt tuyệt đẹp.", "/images/PCA.jpg", null, "PC Gaming Alpha", 28000000m },
                    { 8, 2, "PC chuyên dụng cho dân đồ họa 3D, dựng phim với bộ vi xử lý đa nhân siêu tốc.", "/images/PCB.jpg", null, "PC Đồ Họa Beta", 32000000m },
                    { 9, 2, "Máy tính để bàn cấu hình Core i5 nhỏ gọn, hoạt động êm ái thích hợp cho không gian làm việc.", "/images/PCA2.png", null, "PC Văn Phòng Core i5", 10000000m },
                    { 10, 2, "Máy trạm Workstation Xeon sức mạnh vượt trội cho mọi tác vụ nặng nhất.", "/images/PCB2.png", null, "PC Workstation Xeon", 45000000m },
                    { 11, 3, "Chuột chơi game không dây siêu nhẹ.", "https://picsum.photos/300/300?random=11", null, "Chuột Gaming Logitech", 1200000m },
                    { 12, 3, "Bàn phím cơ cao cấp với đèn LED RGB.", "https://picsum.photos/300/300?random=12", null, "Bàn phím cơ Razer", 2500000m },
                    { 13, 3, "Tai nghe chống ồn chủ động.", "https://picsum.photos/300/300?random=13", null, "Tai nghe Sony", 3000000m },
                    { 14, 4, "Màn hình 27 inch 4K IPS.", "https://picsum.photos/300/300?random=14", null, "Màn hình Dell UltraSharp", 8000000m },
                    { 15, 4, "Ổ cứng SSD NVMe tốc độ cao.", "https://picsum.photos/300/300?random=15", null, "Ổ cứng SSD Samsung 1TB", 2000000m },
                    { 16, 4, "RAM DDR4 3200MHz.", "https://picsum.photos/300/300?random=16", null, "RAM Corsair 16GB", 1500000m },
                    { 17, 4, "Card đồ họa chơi game tầm trung.", "https://picsum.photos/300/300?random=17", null, "Card đồ họa RTX 3060", 9000000m },
                    { 18, 4, "Bo mạch chủ cao cấp cho Intel.", "https://picsum.photos/300/300?random=18", null, "Mainboard ASUS ROG", 4000000m },
                    { 19, 4, "Tản nhiệt AIO có màn hình LCD.", "https://picsum.photos/300/300?random=19", null, "Tản nhiệt nước NZXT", 3500000m },
                    { 20, 4, "Nguồn máy tính chuẩn 80 Plus Gold.", "https://picsum.photos/300/300?random=20", null, "Nguồn Corsair 750W", 2200000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

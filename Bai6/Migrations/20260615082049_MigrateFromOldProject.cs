using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bai6.Migrations
{
    /// <inheritdoc />
    public partial class MigrateFromOldProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Laptop");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Desktop");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Gaming Gear");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Linh kiện");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "Laptop HP Pavilion mỏng nhẹ, hiệu năng cao phù hợp cho sinh viên và dân văn phòng.", "/images/Hp1.png", "Laptop HP Pavilion", 15000000m, 50 },
                    { 2, 1, "Laptop doanh nhân Lenovo ThinkPad siêu bền bỉ, bàn phím gõ cực êm.", "/images/Lap1.png", "Laptop Lenovo ThinkPad", 25000000m, 50 },
                    { 3, 1, "Laptop gaming quốc dân Acer Nitro 5 với tản nhiệt cực tốt và cấu hình mạnh mẽ.", "/images/Nitro1.png", "Laptop Acer Nitro 5", 22000000m, 50 },
                    { 4, 1, "Laptop gaming cao cấp HP OMEN với thiết kế sang trọng, hiệu năng đỉnh cao.", "/images/OM1.png", "Laptop HP OMEN", 35000000m, 50 },
                    { 5, 1, "Laptop chuyên game Asus ROG mang lại trải nghiệm chơi game tuyệt vời.", "/images/asusrog.jpg", "Laptop Asus ROG", 30000000m, 50 },
                    { 6, 1, "Laptop Dell XPS 15 với màn hình vô cực tuyệt đẹp, dành cho người dùng sáng tạo.", "/images/OIP.jpg", "Laptop Dell XPS 15", 40000000m, 50 },
                    { 7, 2, "Bộ PC Gaming Alpha với tản nhiệt nước, vỏ case trong suốt tuyệt đẹp.", "/images/PCA.jpg", "PC Gaming Alpha", 28000000m, 50 },
                    { 8, 2, "PC chuyên dụng cho dân đồ họa 3D, dựng phim với bộ vi xử lý đa nhân siêu tốc.", "/images/PCB.jpg", "PC Đồ Họa Beta", 32000000m, 50 },
                    { 9, 2, "Máy tính để bàn cấu hình Core i5 nhỏ gọn, hoạt động êm ái thích hợp cho không gian làm việc.", "/images/PCA2.png", "PC Văn Phòng Core i5", 10000000m, 50 },
                    { 10, 2, "Máy trạm Workstation Xeon sức mạnh vượt trội cho mọi tác vụ nặng nhất.", "/images/PCB2.png", "PC Workstation Xeon", 45000000m, 50 },
                    { 11, 3, "Chuột chơi game không dây siêu nhẹ.", "/images/Logi1.png", "Chuột Gaming Logitech", 1200000m, 50 },
                    { 12, 3, "Bàn phím cơ cao cấp với đèn LED RGB.", "/images/Razer.png", "Bàn phím cơ Razer", 2500000m, 50 },
                    { 13, 3, "Tai nghe chống ồn chủ động.", "/images/sony-headphone.jpg", "Tai nghe Sony", 3000000m, 50 },
                    { 14, 4, "Màn hình 27 inch 4K IPS.", "/images/dell-ultrasharp.jpg", "Màn hình Dell UltraSharp", 8000000m, 50 },
                    { 15, 4, "Ổ cứng SSD NVMe tốc độ cao.", "/images/samsung-ssd.jpg", "Ổ cứng SSD Samsung 1TB", 2000000m, 50 },
                    { 16, 4, "RAM DDR4 3200MHz.", "/images/corsair-ram.png", "RAM Corsair 16GB", 1500000m, 50 },
                    { 17, 4, "Card đồ họa chơi game tầm trung.", "/images/rtx3060.jpg", "Card đồ họa RTX 3060", 9000000m, 50 },
                    { 18, 4, "Bo mạch chủ cao cấp cho Intel.", "/images/asus-rog-mainboard.jpg", "Mainboard ASUS ROG", 4000000m, 50 },
                    { 19, 4, "Tản nhiệt AIO có màn hình LCD.", "/images/nzxt-aio.jpg", "Tản nhiệt nước NZXT", 3500000m, 50 },
                    { 20, 4, "Nguồn máy tính chuẩn 80 Plus Gold.", "/images/corsair-psu.jpg", "Nguồn Corsair 750W", 2200000m, 50 },
                    { 21, 1, "Laptop Apple với chip M2 siêu mạnh mẽ.", "/images/macbook-neo.jpg", "MacBook Pro M2", 35000000m, 50 },
                    { 22, 1, "Laptop siêu nhẹ chỉ 1.1kg, pin trâu.", "/images/Lap12.png", "Laptop LG Gram 16", 38000000m, 50 },
                    { 23, 2, "Máy tính siêu nhỏ gọn phù hợp tivi phòng khách.", "/images/intel-nuc.jpg", "PC Mini Intel NUC", 12000000m, 50 },
                    { 24, 2, "Máy trạm Apple hiệu năng đồ họa cực đỉnh.", "/images/mac-studio.jpg", "Mac Studio M2 Max", 55000000m, 50 },
                    { 25, 3, "Tay cầm chơi game tốt nhất trên Windows.", "/images/xbox-controller.jpg", "Tay cầm Xbox Series X", 1500000m, 50 },
                    { 26, 3, "Webcam chuẩn Full HD cho streamer.", "/images/logitech-c920.jpg", "Webcam Logitech C920", 1800000m, 50 },
                    { 27, 3, "Loa di động âm thanh sống động.", "/images/jbl-speaker.jpg", "Loa Bluetooth JBL", 2000000m, 50 },
                    { 28, 4, "Ổ cứng lưu trữ dữ liệu dung lượng cao.", "/images/seagate-hdd.jpg", "Ổ cứng HDD Seagate 2TB", 1400000m, 50 },
                    { 29, 4, "Vỏ máy tính bằng thép với mặt kính cường lực.", "/images/nzxt-h510.jpg", "Vỏ Case NZXT H510", 1900000m, 50 },
                    { 30, 4, "Quạt tản nhiệt siêu êm, lưu lượng gió lớn.", "/images/noctua-fan.jpg", "Quạt tản nhiệt Noctua", 800000m, 50 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Sách Giáo Khoa");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Sách Văn Học");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Sách Khoa Học");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Sách Kỹ Năng");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 5, "Sách Thiếu Nhi" });
        }
    }
}

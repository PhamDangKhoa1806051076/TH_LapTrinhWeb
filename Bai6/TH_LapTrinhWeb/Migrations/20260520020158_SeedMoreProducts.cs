using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhamDangKhoa_W345_C2.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "ImageUrls", "Name", "Price" },
                values: new object[,]
                {
                    { 21, 1, "Laptop Apple với chip M2 siêu mạnh mẽ.", "https://picsum.photos/300/300?random=21", null, "MacBook Pro M2", 35000000m },
                    { 22, 1, "Laptop siêu nhẹ chỉ 1.1kg, pin trâu.", "https://picsum.photos/300/300?random=22", null, "Laptop LG Gram 16", 38000000m },
                    { 23, 2, "Máy tính siêu nhỏ gọn phù hợp tivi phòng khách.", "https://picsum.photos/300/300?random=23", null, "PC Mini Intel NUC", 12000000m },
                    { 24, 2, "Máy trạm Apple hiệu năng đồ họa cực đỉnh.", "https://picsum.photos/300/300?random=24", null, "Mac Studio M2 Max", 55000000m },
                    { 25, 3, "Tay cầm chơi game tốt nhất trên Windows.", "https://picsum.photos/300/300?random=25", null, "Tay cầm Xbox Series X", 1500000m },
                    { 26, 3, "Webcam chuẩn Full HD cho streamer.", "https://picsum.photos/300/300?random=26", null, "Webcam Logitech C920", 1800000m },
                    { 27, 3, "Loa di động âm thanh sống động.", "https://picsum.photos/300/300?random=27", null, "Loa Bluetooth JBL", 2000000m },
                    { 28, 4, "Ổ cứng lưu trữ dữ liệu dung lượng cao.", "https://picsum.photos/300/300?random=28", null, "Ổ cứng HDD Seagate 2TB", 1400000m },
                    { 29, 4, "Vỏ máy tính bằng thép với mặt kính cường lực.", "https://picsum.photos/300/300?random=29", null, "Vỏ Case NZXT H510", 1900000m },
                    { 30, 4, "Quạt tản nhiệt siêu êm, lưu lượng gió lớn.", "https://picsum.photos/300/300?random=30", null, "Quạt tản nhiệt Noctua", 800000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

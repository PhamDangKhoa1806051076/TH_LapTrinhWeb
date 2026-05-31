using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhamDangKhoa_W345_C2.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAsusROGImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/asusrog.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/80.jpg");
        }
    }
}

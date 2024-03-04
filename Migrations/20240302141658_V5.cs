using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiAppBangHang.Migrations
{
    public partial class V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductBooks");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "ProductBooks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ProductBooks");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductBooks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

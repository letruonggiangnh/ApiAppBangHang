using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiAppBangHang.Migrations
{
    public partial class V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ProductBooks");

            migrationBuilder.CreateTable(
                name: "BookDescriptions",
                columns: table => new
                {
                    BookDescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookDescriptions", x => x.BookDescriptionId);
                    table.ForeignKey(
                        name: "FK_BookDescriptions_ProductBooks_BookId",
                        column: x => x.BookId,
                        principalTable: "ProductBooks",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookDescriptions_BookId",
                table: "BookDescriptions",
                column: "BookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookDescriptions");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "ProductBooks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

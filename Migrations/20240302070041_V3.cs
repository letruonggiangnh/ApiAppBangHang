using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiAppBangHang.Migrations
{
    public partial class V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookCategoryParents",
                columns: table => new
                {
                    CategoryParentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryParentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategoryParents", x => x.CategoryParentId);
                });

            migrationBuilder.CreateTable(
                name: "BookTags",
                columns: table => new
                {
                    BookTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTags", x => x.BookTagId);
                });

            migrationBuilder.CreateTable(
                name: "BookCategoryChilds",
                columns: table => new
                {
                    CategoryChildId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryParentId = table.Column<int>(type: "int", nullable: false),
                    CategoryChildName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategoryChilds", x => x.CategoryChildId);
                    table.ForeignKey(
                        name: "FK_BookCategoryChilds_BookCategoryParents_CategoryParentId",
                        column: x => x.CategoryParentId,
                        principalTable: "BookCategoryParents",
                        principalColumn: "CategoryParentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductBooks",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishingCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    NumberOfPages = table.Column<long>(type: "bigint", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookPrice = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    SoldQuantity = table.Column<int>(type: "int", nullable: false),
                    CategoryChildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBooks", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_ProductBooks_BookCategoryChilds_CategoryChildId",
                        column: x => x.CategoryChildId,
                        principalTable: "BookCategoryChilds",
                        principalColumn: "CategoryChildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductBooks_BookTags_TagId",
                        column: x => x.TagId,
                        principalTable: "BookTags",
                        principalColumn: "BookTagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCategoryChilds_CategoryParentId",
                table: "BookCategoryChilds",
                column: "CategoryParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBooks_CategoryChildId",
                table: "ProductBooks",
                column: "CategoryChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBooks_TagId",
                table: "ProductBooks",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductBooks");

            migrationBuilder.DropTable(
                name: "BookCategoryChilds");

            migrationBuilder.DropTable(
                name: "BookTags");

            migrationBuilder.DropTable(
                name: "BookCategoryParents");
        }
    }
}

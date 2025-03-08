using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superecommere.Migrations
{
    /// <inheritdoc />
    public partial class StoreCategoryContainer1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreCategoryContainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategoryContainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCategoryContainer_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCategoryContainer_SubStoreCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubStoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryContainer_StoreId",
                table: "StoreCategoryContainer",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryContainer_SubCategoryId",
                table: "StoreCategoryContainer",
                column: "SubCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreCategoryContainer");
        }
    }
}

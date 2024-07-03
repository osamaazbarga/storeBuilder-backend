using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace superecommere.Migrations
{
    /// <inheritdoc />
    public partial class addMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Merchant",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Merchant",
                table: "AspNetUsers");
        }
    }
}

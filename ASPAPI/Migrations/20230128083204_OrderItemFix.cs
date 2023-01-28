using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPAPI.Migrations
{
    /// <inheritdoc />
    public partial class OrderItemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "OrderItems",
                newName: "ProductCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCount",
                table: "OrderItems",
                newName: "Count");
        }
    }
}

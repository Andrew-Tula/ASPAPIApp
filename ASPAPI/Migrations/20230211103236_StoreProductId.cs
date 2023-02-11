using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPAPI.Migrations
{
    /// <inheritdoc />
    public partial class StoreProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreProductId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_StoreProductId",
                table: "OrderItems",
                column: "StoreProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_StoreProducts_StoreProductId",
                table: "OrderItems",
                column: "StoreProductId",
                principalTable: "StoreProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_StoreProducts_StoreProductId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_StoreProductId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "StoreProductId",
                table: "OrderItems");
        }
    }
}

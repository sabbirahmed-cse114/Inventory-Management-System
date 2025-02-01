using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddProductInStockTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "StockTransfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_ProductId",
                table: "StockTransfers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Products_ProductId",
                table: "StockTransfers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Products_ProductId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_ProductId",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "StockTransfers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REC.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class AddStockInStockTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StockId",
                table: "StockTransfers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_StockId",
                table: "StockTransfers",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Stocks_StockId",
                table: "StockTransfers",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Stocks_StockId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_StockId",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "StockTransfers");
        }
    }
}

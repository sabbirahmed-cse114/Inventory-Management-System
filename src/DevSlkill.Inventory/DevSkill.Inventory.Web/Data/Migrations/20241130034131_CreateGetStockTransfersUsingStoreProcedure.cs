using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class CreateGetStockTransfersUsingStoreProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetStockTransfers 
                    @PageIndex INT,
                    @PageSize INT, 
                    @OrderBy NVARCHAR(50),
                    @ProductId UNIQUEIDENTIFIER = NULL,
                    @FromWarehouseId UNIQUEIDENTIFIER = NULL,
                    @ToWarehouseId UNIQUEIDENTIFIER = NULL,
                    @Quantity INT = NULL,
                    @Note NVARCHAR(MAX) = NULL,     
                    @Date DATETIME = NULL,
                    @Total INT OUTPUT,
                    @TotalDisplay INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF @PageIndex IS NULL OR @PageIndex < 1 SET @PageIndex = 1;
                    IF @PageSize IS NULL OR @PageSize < 1 SET @PageSize = 10;

                    IF @OrderBy NOT IN ('ProductName', 'FromWarehouse', 'ToWarehouse', 'Quantity', 'Date')
                        SET @OrderBy = 'ProductName';

                    SELECT @Total = COUNT(*) 
                    FROM Products;

                    DECLARE @countsql NVARCHAR(MAX);
                    DECLARE @countparamlist NVARCHAR(MAX);

                    SET @countsql = 'SELECT @TotalDisplay = COUNT(*)
                                     FROM StockTransfers t
                                     INNER JOIN Products p ON t.ProductId = p.Id
                                     INNER JOIN Warehouses fw ON t.FromWarehouseId = fw.Id
                                     INNER JOIN Warehouses tw ON t.ToWarehouseId = tw.Id
                                     WHERE 1 = 1';

                    IF @ProductId IS NOT NULL
                        SET @countsql += ' AND t.ProductId = @xProductId';
                    IF @FromWarehouseId IS NOT NULL
                        SET @countsql += ' AND t.FromWarehouseId = @xFromWarehouseId';
                    IF @ToWarehouseId IS NOT NULL
                        SET @countsql += ' AND t.ToWarehouseId = @xToWarehouseId';
                    IF @Quantity IS NOT NULL
                        SET @countsql += ' AND t.Quantity = @xQuantity';
                    IF @Note IS NOT NULL
                        SET @countsql += ' AND t.Note LIKE ''%'' + @xNote + ''%''';
                    IF @Date IS NOT NULL
                        SET @countsql += ' AND t.Date = @xDate';

                    SET @countparamlist = '@xProductId UNIQUEIDENTIFIER,
                                           @xFromWarehouseId UNIQUEIDENTIFIER,
                                           @xToWarehouseId UNIQUEIDENTIFIER,
                                           @xQuantity INT,
                                           @xNote NVARCHAR(MAX),
                                           @xDate DATETIME,
                                           @TotalDisplay INT OUTPUT';

                    EXEC sp_executesql @countsql, @countparamlist,
                         @ProductId,
                         @FromWarehouseId,
                         @ToWarehouseId,
                         @Quantity,
                         @Note,
                         @Date,
                         @TotalDisplay = @TotalDisplay OUTPUT;

                    DECLARE @sql NVARCHAR(MAX);
                    DECLARE @paramlist NVARCHAR(MAX);

                    SET @sql = 'SELECT t.Id, p.Name AS ProductName, fw.Name AS FromWarehouse, tw.Name AS ToWarehouse,
                                       t.Quantity, t.Note, CONVERT(DATE, t.Date) AS Date
                                FROM StockTransfers t
                                INNER JOIN Products p ON t.ProductId = p.Id
                                INNER JOIN Warehouses fw ON t.FromWarehouseId = fw.Id
                                INNER JOIN Warehouses tw ON t.ToWarehouseId = tw.Id
                                WHERE 1 = 1';

                    IF @ProductId IS NOT NULL
                        SET @sql += ' AND t.ProductId = @xProductId';
                    IF @FromWarehouseId IS NOT NULL
                        SET @sql += ' AND t.FromWarehouseId = @xFromWarehouseId';
                    IF @ToWarehouseId IS NOT NULL
                        SET @sql += ' AND t.ToWarehouseId = @xToWarehouseId';
                    IF @Quantity IS NOT NULL
                        SET @sql += ' AND t.Quantity = @xQuantity';
                    IF @Note IS NOT NULL
                        SET @sql += ' AND t.Note LIKE ''%'' + @xNote + ''%''';
                    IF @Date IS NOT NULL
                        SET @sql += ' AND t.Date = @xDate';

                    SET @sql += ' ORDER BY ' + @OrderBy + '
                                  OFFSET @PageSize * (@PageIndex - 1) ROWS
                                  FETCH NEXT @PageSize ROWS ONLY';

                    SET @paramlist = '@xProductId UNIQUEIDENTIFIER,
                                      @xFromWarehouseId UNIQUEIDENTIFIER,
                                      @xToWarehouseId UNIQUEIDENTIFIER,
                                      @xQuantity INT,
                                      @xNote NVARCHAR(MAX),
                                      @xDate DATETIME,
                                      @PageIndex INT,
                                      @PageSize INT';

                    EXEC sp_executesql @sql, @paramlist,
                         @ProductId,
                         @FromWarehouseId,
                         @ToWarehouseId,
                         @Quantity,
                         @Note,
                         @Date,
                         @PageIndex,
                         @PageSize;
                END
                GO                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetStockTransfers]";
            migrationBuilder.DropTable(sql);
        }
    }
}

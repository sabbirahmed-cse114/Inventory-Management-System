using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class CreateGetStocksUsingSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE GetStocks 
                      	@PageIndex int,
                      	@PageSize int , 
                      	@OrderBy nvarchar(50),
                		@ProductId uniqueidentifier = NULL,
                		@WarehouseId uniqueidentifier = NULL,
                		@PurchasePrice int = NULL,
                		@SellingPrice int = NULL,
                		@Quantity int = NULL,
                      	@Reason nvarchar(max) = '%',
                		@Note nvarchar(max) = '%',      	
                   		@Date datetime = NULL,
                      	@Total int output,
                      	@TotalDisplay int output
                AS
                BEGIN

                      	SET NOCOUNT ON;

                      	Declare @sql nvarchar(2000);
                      	Declare @countsql nvarchar(2000);
                      	Declare @paramList nvarchar(MAX); 
                      	Declare @countparamList nvarchar(MAX);

                      	-- Collecting Total
                      	Select @Total = count(*) from Products;

                      	-- Collecting Total Display
                      	SET @countsql = 'select @TotalDisplay = count(*) from Stocks s inner join 
                               					Products p on s.ProductId = p.Id inner join
                               					Warehouses w on s.WarehouseId = w.Id where 1 = 1';

                		IF @ProductId IS NOT NULL
                      	SET @countsql = @countsql + ' AND s.ProductId = @xProductId'

                		IF @WarehouseId IS NOT NULL
                      	SET @countsql = @countsql + ' AND s.WarehouseId = @xWarehouseId'

                		IF @PurchasePrice IS NOT NULL
                      	SET @countsql = @countsql + ' AND s.PurchasePrice = @xPurchasePrice'

                		IF @SellingPrice IS NOT NULL
                		SET @countsql = @countsql + ' AND s.SellingPrice = @xSellingPrice'

                		IF @Quantity IS NOT NULL
                		SET @countsql = @countsql + ' AND s.Quantity = @xQuantity'

                      	SET @countsql = @countsql + ' AND s.Reason LIKE ''%'' + @xReason + ''%'''

                		SET @countsql = @countsql + ' AND s.Note LIKE ''%'' + @xNote + ''%'''

                      	IF @Date IS NOT NULL
                      	SET @countsql = @countsql + ' AND s.Date = @xDate'

                      	SELECT @countparamlist = '@xProductId uniqueidentifier,
                					@xWarehouseId uniqueidentifier,
                            		@xPurchasePrice int,
                         			@xSellingPrice int,
                            		@xQuantity int,
                         			@xReason nvarchar(max),
                					@xNote nvarchar(max),
                					@xDate datetime,
                            		@TotalDisplay int output' ;

                      	exec sp_executesql @countsql , @countparamlist ,
                            		@ProductId,
                					@WarehouseId,
                            		@PurchasePrice,
                            		@SellingPrice,
                            		@Quantity,
                         			@Reason,
                					@Note,
                					@Date,
                            		@TotalDisplay = @TotalDisplay output;

                      	-- Collecting Data
                      	SET @sql = 'SELECT s.Id,p.Name AS Product, w.Name AS Warehouse, s.PurchasePrice, s.SellingPrice,
                				    s.Quantity, s.Reason, s.Note, CONVERT(DATE, s.Date) AS Date FROM 
                					Stocks s INNER JOIN Products p ON s.ProductId = p.Id INNER JOIN
                					Warehouses w ON s.WarehouseId = w.Id WHERE 1 = 1';

                		IF @ProductId IS NOT NULL
                      	SET @sql = @sql + ' AND s.ProductId = @xProductId' 

                   		IF @WarehouseId IS NOT NULL
                      	SET @sql = @sql + ' AND s.WarehouseId = @xWarehouseId'

                		IF @PurchasePrice IS NOT NULL
                      	SET @sql = @sql + ' AND s.PurchasePrice = @xPurchasePrice'

                		IF @SellingPrice IS NOT NULL
                      	SET @sql = @sql + ' AND s.SellingPrice = @xSellingPrice'

                		IF @Quantity IS NOT NULL
                      	SET @sql = @sql + ' AND s.Quantity = @xQuantity'

                      	SET @sql = @sql + ' AND s.Reason LIKE ''%'' + @xReason + ''%'''

                		SET @sql = @sql + ' AND s.Note LIKE ''%'' + @xNote + ''%'''

                      	IF @Date IS NOT NULL
                      	SET @sql = @sql + ' AND s.Date = @xDate' 

                      	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                      	ROWS FETCH NEXT @PageSize ROWS ONLY';

                      	SELECT @paramlist = '@xProductId uniqueidentifier,
                         			@xWarehouseId uniqueidentifier,
                					@xPurchasePrice int,
                					@xSellingPrice int,
                            		@xQuantity int,
                         			@xReason nvarchar(max),
                					@xNote nvarchar(max),
                            		@xDate datetime,
                            		@PageIndex int,
                            		@PageSize int' ;

                      	exec sp_executesql @sql , @paramlist ,
                            		@ProductId,
                					@WarehouseId,
                            		@PurchasePrice,
                            		@SellingPrice,
                            		@Quantity,
                         			@Reason,
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
            var sql = "DROP PROCEDURE [dbo].[GetStocks]";
            migrationBuilder.DropTable(sql);
        }
    }
}

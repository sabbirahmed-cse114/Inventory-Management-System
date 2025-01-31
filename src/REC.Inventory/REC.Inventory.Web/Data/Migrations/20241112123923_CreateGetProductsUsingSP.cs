using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REC.Inventory.Web.Migrations.InventoryDb
{
    /// <inheritdoc />
    public partial class CreateGetProductsUsingSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                    CREATE OR ALTER PROCEDURE GetProducts 
                      	@PageIndex int,
                      	@PageSize int , 
                      	@OrderBy nvarchar(50),
                      	@Name nvarchar(max) = '%',
                		@ImagePath nvarchar(max) = '%',
                      	@Barcode nvarchar(max) = '%',
                      	@Status nvarchar(max) = '%',
                      	@CategoryId uniqueidentifier = NULL,
                   		@UnitId uniqueidentifier = NULL,
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
                      	SET @countsql = 'select @TotalDisplay = count(*) from Products p inner join 
                               					Categories c on p.CategoryId = c.Id inner join
                               					Units u on p.UnitId = u.Id where 1 = 1';

                      	SET @countsql = @countsql + ' AND p.Name LIKE ''%'' + @xName + ''%'''

                		SET @countsql = @countsql + ' AND p.ImagePath LIKE ''%'' + @xImagePath + ''%'''

                		SET @countsql = @countsql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%'''

                      	SET @countsql = @countsql + ' AND p.Status LIKE ''%'' + @xStatus + ''%''' 

                      	IF @CategoryId IS NOT NULL
                      	SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId'

                   		IF @UnitId IS NOT NULL
                      	SET @countsql = @countsql + ' AND p.UnitId = @xUnitId'

                      	SELECT @countparamlist = '@xName nvarchar(max),
                					@xImagePath nvarchar(max),
                            		@xBarcode nvarchar(max),
                         			@xStatus nvarchar(max),
                            		@xCategoryId uniqueidentifier,
                         			@xUnitId uniqueidentifier,
                            		@TotalDisplay int output' ;

                      	exec sp_executesql @countsql , @countparamlist ,
                            		@Name,
                					@ImagePath,
                            		@Barcode,
                            		@Status,
                            		@CategoryId,
                         			@UnitId,
                            		@TotalDisplay = @TotalDisplay output;

                      	-- Collecting Data
                      	SET @sql = 'select p.Id,p.Name,p.ImagePath,p.Barcode,p.Status,
                                    c.Name as Category,u.Name as Unit from Products p inner join 
                                    Categories c on p.CategoryId = c.Id inner join 
                                	Units u on p.UnitId = u.Id where 1 = 1';

                      	SET @sql = @sql + ' AND p.Name LIKE ''%'' + @xName + ''%'''

                		SET @sql = @sql + ' AND p.ImagePath LIKE ''%'' + @xImagePath + ''%'''

                      	SET @sql = @sql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''' 

                    	SET @sql = @sql + ' AND p.Status LIKE ''%'' + @xStatus + ''%''' 

                      	IF @CategoryId IS NOT NULL
                      	SET @sql = @sql + ' AND p.CategoryId = @xCategoryId' 

                   		IF @UnitId IS NOT NULL
                      	SET @sql = @sql + ' AND p.UnitId = @xUnitId'

                      	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                      	ROWS FETCH NEXT @PageSize ROWS ONLY';

                      	SELECT @paramlist = '@xName nvarchar(max),
                					@xImagePath nvarchar(max),
                            		@xBarcode nvarchar(max),
                         			@xStatus nvarchar(max),
                            		@xCategoryId uniqueidentifier,
                         			@xUnitId uniqueidentifier,
                            		@PageIndex int,
                            		@PageSize int' ;

                      	exec sp_executesql @sql , @paramlist ,
                            		@Name,
                					@ImagePath,
                            		@Barcode,
                            		@Status,
                            		@CategoryId,
                         			@UnitId,
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
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);
        }
    }
}

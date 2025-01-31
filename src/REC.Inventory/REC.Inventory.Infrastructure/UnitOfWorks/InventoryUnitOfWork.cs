using REC.Inventory.Application;
using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.RepositoryContracts;

namespace REC.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public IUnitRepository UnitRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IStockRepository StockRepository { get; private set; }
        public ITransferRepository TransferRepository { get; private set; }

        public InventoryUnitOfWork(InventoryDbContext dbContext,
            IProductRepository productRepository,
            IUnitRepository unitRepository,
            ICategoryRepository categoryRepository,
            IWarehouseRepository warehouseRepository,
            IStockRepository stockRepository,
            ITransferRepository transferRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
            UnitRepository = unitRepository;
            CategoryRepository = categoryRepository;
            WarehouseRepository = warehouseRepository;
            StockRepository = stockRepository;
            TransferRepository = transferRepository;
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetPagedProductUsingSPAsync(int pageIndex,
            int pageSize, ProductSearchDto search, string? order)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "Name", search.Name == string.Empty ? null : search.Name },
                    { "Barcode", search.Barcode == string.Empty ? null : search.Barcode },
                    { "Status", search.Status == string.Empty ? null : search.Status },
                    { "CategoryId", search.CategoryId == Guid.Empty ? null : search.CategoryId },
                    { "UnitId", search.UnitId == Guid.Empty ? null : search.UnitId }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<StockDto> data, int total, int totalDisplay)> GetPagedStockAsync(int pageIndex,
            int pageSize, StockSearchDto search, string? order)
        {
            var procedureName = "GetStocks";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<StockDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order },
                    { "ProductId", search.ProductId == Guid.Empty ? null : search.ProductId },
                    { "WarehouseId", search.WarehouseId == Guid.Empty ? null : search.WarehouseId }
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<TransferDto> data, int total, int totalDisplay)> GetPagedTransferAsync(int pageIndex,
            int pageSize, TransferSearchDto search, string? order)
        {
            var procedureName = "GetStockTransfers";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<TransferDto>(procedureName,
                new Dictionary<string, object>
                {
                    { "PageIndex", pageIndex },
                    { "PageSize", pageSize },
                    { "OrderBy", order }					 
                },
                new Dictionary<string, Type>
                {
                    { "Total", typeof(int) },
                    { "TotalDisplay", typeof(int) },
                });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}

using REC.Inventory.Domain;
using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.RepositoryContracts;

namespace REC.Inventory.Application
{
    public interface IInventoryUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IUnitRepository UnitRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }
        IStockRepository StockRepository { get; }
        ITransferRepository TransferRepository { get; }

        Task<(IList<ProductDto> data, int total, int totalDisplay)> GetPagedProductUsingSPAsync(int pageIndex,
            int pageSize, ProductSearchDto search, string? order);
        Task<(IList<StockDto> data, int total, int totalDisplay)> GetPagedStockAsync(int pageIndex,
           int pageSize, StockSearchDto search, string? order);
        Task<(IList<TransferDto> data, int total, int totalDisplay)> GetPagedTransferAsync(int pageIndex,
           int pageSize, TransferSearchDto search, string? order);
    }
}

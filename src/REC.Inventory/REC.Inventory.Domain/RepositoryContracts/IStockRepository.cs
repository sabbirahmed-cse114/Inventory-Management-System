using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Domain.RepositoryContracts
{
    public interface IStockRepository : IRepositoryBase<Stock,Guid>
	{
		Task<Stock> GetStockAsync(Guid id);
        Task<Stock> GetStockByProductAndWarehouseAsync(Guid productId, Guid warehouseId);

    }
}

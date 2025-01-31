using Microsoft.EntityFrameworkCore;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Domain.RepositoryContracts;

namespace REC.Inventory.Infrastructure.Repositories
{
    public class StockRepository : Repository<Stock,Guid>, IStockRepository
	{
		public StockRepository(InventoryDbContext context) : base(context)
		{

		}

		public async Task<Stock> GetStockAsync(Guid id)
		{
			return (await GetAsync(x => x.Id == id, y => y.Include(z => z.Warehouse).Include(z => z.Product))).FirstOrDefault();
		}
        public async Task<Stock> GetStockByProductAndWarehouseAsync(Guid productId, Guid warehouseId)
        {
            return (await GetAsync(x => x.Product.Id == productId && x.Warehouse.Id == warehouseId, y => y.Include(z => z.Warehouse).Include(z => z.Product))).FirstOrDefault();
        }
    }
}

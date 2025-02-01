using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class StockManagementService : IStockManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public StockManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public void CreateStock(Stock stock)
        {
            _inventoryUnitOfWork.StockRepository.Add(stock);
            _inventoryUnitOfWork.Save();
        }

        public void DeleteStock(Guid id)
        {
            _inventoryUnitOfWork.StockRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

        public async Task<Stock> GetStockInformationAsync(Guid id)
        {
            return await _inventoryUnitOfWork.StockRepository.GetStockAsync(id);
        }

        public async Task<Stock> GetStockInformationUsingProductAndWarehouseAsync(Guid product,Guid warehouse)
        {
            return await _inventoryUnitOfWork.StockRepository.GetStockByProductAndWarehouseAsync(product, warehouse);
        }

        public async Task<(IList<StockDto> data, int total, int totalDisplay)> GetStocksSP(int pageIndex, int pageSize, StockSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedStockAsync(pageIndex, pageSize, search, order);
        }
		public Stock GetStock(Guid Id)
		{
			return _inventoryUnitOfWork.StockRepository.GetById(Id);
		}
		public void UpdateStock(Stock stock)
		{

			_inventoryUnitOfWork.StockRepository.Edit(stock);
			_inventoryUnitOfWork.Save();
		}
	}
}

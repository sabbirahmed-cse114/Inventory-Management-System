using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public interface IStockManagementService
    {
        void CreateStock(Stock stock);
        Task<(IList<StockDto> data, int total, int totalDisplay)> GetStocksSP(int pageIndex, int pageSize,
            StockSearchDto search, string? order);
        Task<Stock> GetStockInformationAsync(Guid id);
        Task<Stock> GetStockInformationUsingProductAndWarehouseAsync(Guid product, Guid warehouse);
        void DeleteStock(Guid id);
		Stock GetStock(Guid Id);
		void UpdateStock(Stock stock);
	}
}

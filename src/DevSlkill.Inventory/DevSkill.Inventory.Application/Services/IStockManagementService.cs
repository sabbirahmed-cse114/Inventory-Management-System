using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
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

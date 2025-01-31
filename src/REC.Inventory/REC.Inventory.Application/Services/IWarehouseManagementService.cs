using REC.Inventory.Domain;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public interface IWarehouseManagementService
    {
        void CreateWarehouse(Warehouse warehouse);
        (IList<Warehouse> data, int total, int totalDisplay) GetWarehouses(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        IList<Warehouse> GetWarehouses();
        Warehouse GetWarehouse(Guid Id);
        void UpdateWarehouse(Warehouse warehouse);
        void DeleteWarehouse(Guid id);
        Task<Warehouse> GetWarehouseInformationAsync(Guid id);
    }
}

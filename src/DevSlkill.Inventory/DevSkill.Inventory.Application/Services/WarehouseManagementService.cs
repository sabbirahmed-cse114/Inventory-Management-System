using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class WarehouseManagementService : IWarehouseManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public WarehouseManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public void CreateWarehouse(Warehouse warehouse)
        {
            if (!_inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicate(warehouse.Name))
            {
                _inventoryUnitOfWork.WarehouseRepository.Add(warehouse);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Warehouse Name should be unique.");
            }
        }

        public void DeleteWarehouse(Guid id)
        {
            _inventoryUnitOfWork.WarehouseRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

        public Warehouse GetWarehouse(Guid Id)
        {
            return _inventoryUnitOfWork.WarehouseRepository.GetById(Id);
        }

        public (IList<Warehouse> data, int total, int totalDisplay) GetWarehouses(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.WarehouseRepository.GetPagedWarehouse(pageIndex, pageSize, search, order);
        }

        public IList<Warehouse> GetWarehouses()
        {
            return _inventoryUnitOfWork.WarehouseRepository.GetAll();
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            if (!_inventoryUnitOfWork.WarehouseRepository.IsTitleDuplicate(warehouse.Name, warehouse.Id))
            {
                _inventoryUnitOfWork.WarehouseRepository.Edit(warehouse);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Warehouse Name should be unique.");
            }
        }
        public async Task<Warehouse> GetWarehouseInformationAsync(Guid id)
        {
            return await _inventoryUnitOfWork.WarehouseRepository.GetWarehouseAsync(id);
        }
    }
}

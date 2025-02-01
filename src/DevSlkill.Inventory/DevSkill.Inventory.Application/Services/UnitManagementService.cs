using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
	public class UnitManagementService : IUnitManagementService
	{
		private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
		public UnitManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
		{
			_inventoryUnitOfWork = inventoryUnitOfWork;
		}

		public IList<Unit> GetUnits()
		{
			return _inventoryUnitOfWork.UnitRepository.GetAll();
		}

		public Unit GetUnit(Guid Id)
		{
			return _inventoryUnitOfWork.UnitRepository.GetById(Id);
		}
		public (IList<Unit> data, int total, int totalDisplay) GetUnits(int pageIndex, int pageSize, DataTablesSearch search, string? order)
		{
			return _inventoryUnitOfWork.UnitRepository.GetPagedUnits(pageIndex, pageSize, search, order);
		}

		public void Create(Unit unit)
		{
            _inventoryUnitOfWork.UnitRepository.Add(unit);
            _inventoryUnitOfWork.Save();
        }
        public void UpdateUnit(Unit unit)
        {
            if (!_inventoryUnitOfWork.UnitRepository.IsTitleDuplicate(unit.Name, unit.Id))
            {
                _inventoryUnitOfWork.UnitRepository.Edit(unit);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Unit Name should be unique.");
            }
        }
        public void DeleteUnit(Guid id)
        {
            _inventoryUnitOfWork.UnitRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }
    }
}

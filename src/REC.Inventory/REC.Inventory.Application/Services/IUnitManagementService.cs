using REC.Inventory.Domain;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public interface IUnitManagementService
    {
		void Create(Unit unit);
		(IList<Unit> data, int total, int totalDisplay) GetUnits(int pageIndex, int pageSize,
			DataTablesSearch search, string? order);
		IList<Unit> GetUnits();
		Unit GetUnit(Guid Id);
        void UpdateUnit(Unit unit);
        void DeleteUnit(Guid id);
    }
}

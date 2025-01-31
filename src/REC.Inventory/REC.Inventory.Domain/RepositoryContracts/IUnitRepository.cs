using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Domain.RepositoryContracts
{
    public interface IUnitRepository : IRepositoryBase<Unit,Guid>
    {
        bool IsTitleDuplicate(string Name, Guid? id = null);
		(IList<Unit> data, int total, int totalDisplay) GetPagedUnits(int pageIndex, int pageSize,
			DataTablesSearch search, string? order);
	}
}

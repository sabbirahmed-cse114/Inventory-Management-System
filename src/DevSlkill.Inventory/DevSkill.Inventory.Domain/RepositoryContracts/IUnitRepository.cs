using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IUnitRepository : IRepositoryBase<Unit,Guid>
    {
        bool IsTitleDuplicate(string Name, Guid? id = null);
		(IList<Unit> data, int total, int totalDisplay) GetPagedUnits(int pageIndex, int pageSize,
			DataTablesSearch search, string? order);
	}
}

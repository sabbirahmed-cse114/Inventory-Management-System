using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
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

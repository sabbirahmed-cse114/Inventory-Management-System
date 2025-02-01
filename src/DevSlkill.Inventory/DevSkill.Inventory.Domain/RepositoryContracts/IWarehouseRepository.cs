using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface IWarehouseRepository : IRepositoryBase<Warehouse,Guid>
    {
        bool IsTitleDuplicate(string Name, Guid? id = null);
        (IList<Warehouse> data, int total, int totalDisplay) GetPagedWarehouse(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        Task<Warehouse> GetWarehouseAsync(Guid id);
    }
}

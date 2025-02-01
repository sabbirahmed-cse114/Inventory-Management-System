using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IRepositoryBase<Category,Guid>
    {
        bool IsTitleDuplicate(string Name, Guid? id = null);
        (IList<Category> data, int total, int totalDisplay) GetPagedCategories(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
    }
}

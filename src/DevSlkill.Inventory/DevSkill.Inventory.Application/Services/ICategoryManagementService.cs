using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface ICategoryManagementService
    {
        void Create(Category category);
        (IList<Category> data, int total, int totalDisplay) GetCategories(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        IList<Category> GetCategories();
        Category GetCategory(Guid Id);
        void UpdateCategory(Category category);
        void DeleteCategory(Guid id);
    }
}

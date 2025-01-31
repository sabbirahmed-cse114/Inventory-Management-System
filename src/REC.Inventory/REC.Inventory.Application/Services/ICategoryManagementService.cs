using REC.Inventory.Domain;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
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

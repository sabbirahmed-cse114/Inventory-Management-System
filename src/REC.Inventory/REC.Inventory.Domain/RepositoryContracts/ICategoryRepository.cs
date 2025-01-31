using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Domain.RepositoryContracts
{
    public interface ICategoryRepository : IRepositoryBase<Category,Guid>
    {
        bool IsTitleDuplicate(string Name, Guid? id = null);
        (IList<Category> data, int total, int totalDisplay) GetPagedCategories(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
    }
}

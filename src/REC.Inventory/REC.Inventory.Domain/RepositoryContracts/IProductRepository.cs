using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Domain.RepositoryContracts
{
    public interface IProductRepository : IRepositoryBase<Product,Guid>
    {
        bool IsTitleDuplicate(string Name,Guid? id = null);
        (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order);
        Task<Product> GetProductAsync(Guid id);
    }
}

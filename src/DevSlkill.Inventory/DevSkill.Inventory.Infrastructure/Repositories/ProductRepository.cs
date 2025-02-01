using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(InventoryDbContext context) : base(context)
        {

        }
        public bool IsTitleDuplicate(string Name, Guid? id = null)
        {
            if(id.HasValue)
            {
                return GetCount(x => x.Id == id.Value && x.Name == Name) > 0;
            }
            else
            {
                return GetCount(x => x.Name == Name) > 0;
            }
        }
        public (IList<Product> data, int total, int totalDisplay) GetPagedProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value), order, null, pageIndex, pageSize, true);
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.Category).Include(z => z.Unit))).FirstOrDefault();
        }
    }
}

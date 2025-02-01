using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category,Guid>, ICategoryRepository
    {
        public CategoryRepository(InventoryDbContext context) : base(context)
        {
        }
        public bool IsTitleDuplicate(string Name, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id == id.Value && x.Name == Name) > 0;
            }
            else
            {
                return GetCount(x => x.Name == Name) > 0;
            }
        }
        public (IList<Category> data, int total, int totalDisplay) GetPagedCategories(int pageIndex, int pageSize,
            DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            else
                return GetDynamic(x => x.Name.Contains(search.Value), order, null, pageIndex, pageSize, true);
        }
    }
}

using REC.Inventory.Domain;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public CategoryManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public IList<Category> GetCategories()
        {
            return _inventoryUnitOfWork.CategoryRepository.GetAll();
        }
        public Category GetCategory(Guid Id)
        {
            return _inventoryUnitOfWork.CategoryRepository.GetById(Id);
        }
        public (IList<Category> data, int total, int totalDisplay) GetCategories(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.CategoryRepository.GetPagedCategories(pageIndex, pageSize, search, order);
        }

        public void Create(Category category)
        {
            if (!_inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(category.Name))
            {
                _inventoryUnitOfWork.CategoryRepository.Add(category);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Category should be unique.");
            }
        }
        public void UpdateCategory(Category category)
        {
            if (!_inventoryUnitOfWork.CategoryRepository.IsTitleDuplicate(category.Name, category.Id))
            {
                _inventoryUnitOfWork.CategoryRepository.Edit(category);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Category should be unique.");
            }
        }
        public void DeleteCategory(Guid id)
        {
            _inventoryUnitOfWork.CategoryRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }
    }
}

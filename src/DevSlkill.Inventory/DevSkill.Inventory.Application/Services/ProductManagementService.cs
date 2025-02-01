using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ProductManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public void CreateProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name))
            {
                _inventoryUnitOfWork.ProductRepository.Add(product);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Product name should be unique.");
            }
        }

        public void DeleteProduct(Guid id)
        {
            _inventoryUnitOfWork.ProductRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

        public async Task<Product> GetProductInformationAsync(Guid id)
        {
            return await _inventoryUnitOfWork.ProductRepository.GetProductAsync(id);
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize, ProductSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedProductUsingSPAsync(pageIndex, pageSize, search, order);
        }
        public void UpdateProduct(Product product)
        {
            if (!_inventoryUnitOfWork.ProductRepository.IsTitleDuplicate(product.Name, product.Id))
            {
                _inventoryUnitOfWork.ProductRepository.Edit(product);
                _inventoryUnitOfWork.Save();
            }
            else
            {
                throw new InvalidOperationException("Name should be unique.");
            }
        }
        public IList<Product> GetProducts()
        {
            return _inventoryUnitOfWork.ProductRepository.GetAll();
        }
        public Product GetProduct(Guid Id)
        {
            return _inventoryUnitOfWork.ProductRepository.GetById(Id);
        }
    }
}

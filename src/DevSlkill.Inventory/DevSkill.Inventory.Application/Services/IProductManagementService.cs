using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public interface IProductManagementService
    {
        void CreateProduct(Product product);
        Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
            ProductSearchDto search, string? order);
        Task<Product> GetProductInformationAsync(Guid id);
        void UpdateProduct(Product product);
        void DeleteProduct(Guid id);
        IList<Product> GetProducts();
        Product GetProduct(Guid Id);
    }
}
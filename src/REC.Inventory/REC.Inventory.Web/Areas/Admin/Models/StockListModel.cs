using Microsoft.AspNetCore.Mvc.Rendering;
using REC.Inventory.Domain;
using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Infrastructure;

namespace REC.Inventory.Web.Areas.Admin.Models
{
    public class StockListModel : DataTables
    {
        public StockSearchDto? SearchItem { get; set; }
        public IList<SelectListItem>? Warehouses { get; private set; }
        public IList<SelectListItem>? Products { get; private set; }

        public void SetWarehouseValues(IList<Warehouse> warehouses)
        {
            Warehouses = RazorUtility.ConvertWarehouses(warehouses);
        }
        public void SetProductValues(IList<Product> products)
        {
            Products = RazorUtility.ConvertProducts(products);
        }
    }
}

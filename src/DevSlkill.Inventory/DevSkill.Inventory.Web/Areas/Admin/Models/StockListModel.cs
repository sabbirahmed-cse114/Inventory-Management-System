using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
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

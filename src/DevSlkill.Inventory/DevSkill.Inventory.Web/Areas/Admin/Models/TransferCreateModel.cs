using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class TransferCreateModel
    {
        [Required]
        public int AvailableStock { get; set; }
        [Required]
        public int SellPrice { get; set; }
        [Required]
        public double Total { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        [StringLength(300)]
        public string? Note { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid FromWarehouseId { get; set; }

        [Required]
        public Guid ToWarehouseId { get; set; }

        public IList<SelectListItem>? FromWarehouse { get; private set; }
        public IList<SelectListItem>? ToWarehouse { get; private set; }
        public IList<SelectListItem>? Products { get; private set; }

        public void SetFromWarehouseValues(IList<Warehouse> fromWarehouse)
        {
            FromWarehouse = RazorUtility.ConvertWarehouses(fromWarehouse);
        }
        public void SetToWarehouseValues(IList<Warehouse> toWarehouse)
        {
            ToWarehouse = RazorUtility.ConvertWarehouses(toWarehouse);
        }
        public void SetProductValues(IList<Product> products)
        {
            Products = RazorUtility.ConvertProducts(products);
        }
        public void SetDateOnlyValues()
        {
            Date = RazorUtility.ConvertDateOnly();
        }
        public double Price(int Quantity,int SellPrice)
        {
            Total = SellPrice * Quantity;
            return Total;
        }
    }
}

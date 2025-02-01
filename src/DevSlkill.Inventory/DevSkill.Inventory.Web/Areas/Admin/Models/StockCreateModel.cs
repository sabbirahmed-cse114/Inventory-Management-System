using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class StockCreateModel
	{
		[Required]
		public DateTime Date { get; set; }

        [Required]
        public int PurchasePrice { get; set; }

		[Required]
		public int Profit { get; set; }
        
        [Required]
        public int SellingPrice { get; set; }

        [Required]
		public int? Quantity { get; set; }

		[StringLength(300)]
		public string? Note { get; set; }

		[Required]
		public Guid ProductId { get; set; }

		[Required]
		public Guid WarehouseId { get; set; }

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
		public void SetDateOnlyValues()
		{
			Date = RazorUtility.ConvertDateOnly();
		}
        public int ProfitCalculation(int Profit, int PurchasePrice)
        {
            double profitCalculation = PurchasePrice * (Profit / 100.00);
            SellingPrice = (int)profitCalculation + PurchasePrice;
			return SellingPrice;
        }
    }
}

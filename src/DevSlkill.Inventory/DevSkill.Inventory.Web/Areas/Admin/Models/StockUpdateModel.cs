using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
	public class StockUpdateModel
	{
		public Guid Id { get; set; }
		[Required]
		public Guid ProductId { get; set; }
		[Required]
		public Guid WarehouseId { get; set; }
		[Required]
		public int PurchasePrice { get; set; }
		[Required]
		public int SellingPrice { get; set; }

		[Required]
		public int Profit {  get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public string? Reason { get; set; }
		[Required]
		public string? Note { get; set; }
		[Required]
		public DateTime Date { get; set; }
        public string? ProductName { get; set; }
		public string? WarehouseName { get; set; }
		[Required]
		public int AddStock { get; set; }
		[Required]
		public int CostPerUnit { get; set; }
		public void SetDateOnlyValues()
		{
			Date = RazorUtility.ConvertDateOnly();
		}
        public int ProfitCalculation()
        {
            double profitCalculation = CurrentPriceCalculation() * (Profit / 100.00);
            SellingPrice = (int)profitCalculation + CurrentPriceCalculation();
            return SellingPrice;
        }
		public int CurrentPriceCalculation()
		{
			int totalPrice = (CostPerUnit * AddStock) + (Quantity * PurchasePrice);
			double total = totalPrice * 1.00;
			int totalStock = AddStock + Quantity;
            double avg = total / totalStock;
			return (int)avg;

		}
    }
}

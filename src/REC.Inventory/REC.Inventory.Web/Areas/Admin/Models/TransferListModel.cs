using Microsoft.AspNetCore.Mvc.Rendering;
using REC.Inventory.Domain;
using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;
using REC.Inventory.Infrastructure;

namespace REC.Inventory.Web.Areas.Admin.Models
{
	public class TransferListModel : DataTables
	{
		public TransferSearchDto SearchItem { get; set; }
		public Guid ProductId { get; set; }
		public Guid FromWarehouseId { get; set; }
		public Guid ToWarehouseId { get; set; }

		public IList<SelectListItem>? Products { get; private set; }
		public IList<SelectListItem>? FromWarehouse { get; private set; }
		public IList<SelectListItem>? ToWarehouse { get; private set; }

		public void SetProductValues(IList<Product> products)
		{
			Products = RazorUtility.ConvertProducts(products);
		}
		public void SetFromWarehouseValues(IList<Warehouse> fromWarehouse)
		{
			FromWarehouse = RazorUtility.ConvertWarehouses(fromWarehouse);
		}
		public void SetToWarehouseValues(IList<Warehouse> toWarehouse)
		{
			ToWarehouse = RazorUtility.ConvertWarehouses(toWarehouse);
		}
	}
}

namespace REC.Inventory.Domain.Dtos
{
	public class StockSearchDto
	{
		public Guid? ProductId { get; set; }
		public Guid? WarehouseId { get; set; }
		public int PurchasePrice { get; set; }
		public int SellingPrice { get; set; }
		public int Quantity { get; set; }
		public string? Reason { get; set; }
		public string? Note { get; set; }
		public DateTime Date { get; set; }
	}
}

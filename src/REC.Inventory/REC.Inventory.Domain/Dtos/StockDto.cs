namespace REC.Inventory.Domain.Dtos
{
	public class StockDto
	{
		public Guid Id { get; set; }
		public string Product { get; set; }
		public string Warehouse {  get; set; }
		public int PurchasePrice { get; set; }
		public int SellingPrice { get; set; }
		public int Quantity { get; set; }
		public string Reason { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
	}
}

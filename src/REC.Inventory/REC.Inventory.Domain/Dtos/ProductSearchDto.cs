namespace REC.Inventory.Domain.Dtos
{
	public class ProductSearchDto
	{
		public string? Name { get; set; }
		public string? Barcode { get; set; }
		public string? Status { get; set; }
		public Guid? CategoryId { get; set; }
		public Guid? UnitId { get; set; }
	}
}

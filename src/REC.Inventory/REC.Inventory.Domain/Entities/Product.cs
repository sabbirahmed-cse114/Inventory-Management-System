namespace REC.Inventory.Domain.Entities
{
    public class Product : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
		public string? ImagePath { get; set; }
		public string? Barcode { get; set; }
        public string? Status { get; set; }
        public Category? Category { get; set; }
        public Unit? Unit { get; set; }
    }
}

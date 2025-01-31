namespace REC.Inventory.Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Barcode { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
    }
}

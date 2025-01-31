namespace REC.Inventory.Domain.Dtos
{
    public class TransferDto
    {
        public Guid Id { get; set; }
		public string ProductName { get; set; }
		public string FromWarehouse { get; set; } 
        public string ToWarehouse { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public DateTime Date {  get; set; }
    }
}

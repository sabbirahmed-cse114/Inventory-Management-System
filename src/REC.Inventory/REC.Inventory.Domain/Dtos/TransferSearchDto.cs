namespace REC.Inventory.Domain.Dtos
{
    public class TransferSearchDto
    {
        public string? FromWarehouse {  get; set; }
        public string? ToWarehouse { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
    }
}

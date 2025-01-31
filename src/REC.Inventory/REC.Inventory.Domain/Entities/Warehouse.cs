namespace REC.Inventory.Domain.Entities
{
    public class Warehouse : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}

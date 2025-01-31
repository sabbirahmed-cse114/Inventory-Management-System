using REC.Inventory.Domain.Entities;


namespace REC.Inventory.Domain.RepositoryContracts
{
    public interface ITransferRepository : IRepositoryBase<StockTransfer,Guid>
    {
        Task<StockTransfer> GetTransferAsync(Guid id);
    }
}

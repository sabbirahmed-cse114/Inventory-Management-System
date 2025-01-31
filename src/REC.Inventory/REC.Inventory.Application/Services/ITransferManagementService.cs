using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public interface ITransferManagementService
    {
        void CreateTransfer(StockTransfer stockTransfer);
        Task<(IList<TransferDto> data, int total, int totalDisplay)> GetTransfer(int pageIndex, int pageSize,
            TransferSearchDto search, string? order);
        Task<StockTransfer> GetTransferInformationAsync(Guid id);
        void DeleteTransfer(Guid id);
        StockTransfer GetTransfer(Guid Id);
    }
}

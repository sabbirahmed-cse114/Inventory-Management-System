using REC.Inventory.Domain.Dtos;
using REC.Inventory.Domain.Entities;

namespace REC.Inventory.Application.Services
{
    public class TransferManagementService : ITransferManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public TransferManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public void CreateTransfer(StockTransfer transfer)
        {
            _inventoryUnitOfWork.TransferRepository.Add(transfer);
            _inventoryUnitOfWork.Save();
        }

        public void DeleteTransfer(Guid id)
        {
            _inventoryUnitOfWork.TransferRepository.Remove(id);
            _inventoryUnitOfWork.Save();
        }

        public async Task<(IList<TransferDto> data, int total, int totalDisplay)> GetTransfer(int pageIndex, int pageSize, TransferSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedTransferAsync(pageIndex, pageSize, search, order);
        }

        public StockTransfer GetTransfer(Guid Id)
        {
            return _inventoryUnitOfWork.TransferRepository.GetById(Id);
        }

        public async Task<StockTransfer> GetTransferInformationAsync(Guid id)
        {
            return await _inventoryUnitOfWork.TransferRepository.GetTransferAsync(id);
        }
    }
}

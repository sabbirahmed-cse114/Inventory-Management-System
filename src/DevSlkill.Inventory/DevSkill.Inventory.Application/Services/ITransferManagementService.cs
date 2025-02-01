using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
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

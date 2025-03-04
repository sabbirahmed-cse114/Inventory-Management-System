﻿using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class TransferRepository : Repository<StockTransfer, Guid>, ITransferRepository
    {
        public TransferRepository(InventoryDbContext context) : base(context)
        {

        }

        public async Task<StockTransfer> GetTransferAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.FromWarehouse).Include(z => z.ToWarehouse).Include(z => z.Product).Include(z => z.Stock))).FirstOrDefault();
        }
    }
}

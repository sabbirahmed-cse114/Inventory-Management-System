using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class StockTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }
		public Product? Product { get; set; }
		public Warehouse? FromWarehouse { get; set; }
        public Warehouse? ToWarehouse { get; set; }
        public Stock? Stock { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REC.Inventory.Domain.Entities
{
    public class Stock : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public Warehouse? Warehouse { get; set; }
        public int PurchasePrice { get; set; }
        public int SellingPrice { get; set; }
        public int WantToProfit { get; set; }
        public int Quantity { get; set; }
        public string? Reason { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
    }
}

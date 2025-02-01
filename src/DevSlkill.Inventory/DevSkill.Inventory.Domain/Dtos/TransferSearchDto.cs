using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class TransferSearchDto
    {
        public string? FromWarehouse {  get; set; }
        public string? ToWarehouse { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
    }
}

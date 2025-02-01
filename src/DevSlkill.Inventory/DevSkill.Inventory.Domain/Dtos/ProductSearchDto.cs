using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
	public class ProductSearchDto
	{
		public string? Name { get; set; }
		public string? Barcode { get; set; }
		public string? Status { get; set; }
		public Guid? CategoryId { get; set; }
		public Guid? UnitId { get; set; }
	}
}
